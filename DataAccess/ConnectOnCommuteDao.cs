using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using ConnectOnCommuteBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConnectOnCommuteBackend.DataAccess
{
    public interface IConnectOnCommuteDao
    {
        Account AddAccount(Account user);
        UserPosition AddUserPosition(UserPosition position);
        bool ConnectWithUser(int accountId, int targetId);
        AccountNotification EmitNotification(AccountNotification notification);
        Account GetAccountById(int userId);
        Account GetAccountByLogin(string email, string password);
        List<AccountConnection> GetAccountConnections(int userId);
        List<Account> GetAllAccounts();
        List<AccountNotification> GetAvailableNotifications(int accountId);
        Account GetNearestPerson(int userId);
        List<Account> GetPeopleNearUser(int userId);
        Account GetUserByEmail(string email);
        bool HasConnected(int accountId, int targetId);
        Account UpdateAccount(Account user);
    }
    public class ConnectOnCommuteDao : IConnectOnCommuteDao
    {
        private readonly DbConnectOnCommute _dbConnectOnCommute;
        private readonly ILogger<ConnectOnCommuteDao> _logger;
        private readonly IConfiguration _config;

        public ConnectOnCommuteDao(IConfiguration config, DbConnectOnCommute dbConnectOnCommute, ILogger<ConnectOnCommuteDao> logger)
        {
            _config = config;
            _dbConnectOnCommute = dbConnectOnCommute;
            _logger = logger;
        }

        public Account GetAccountByLogin(string email, string password)
        {
            return _dbConnectOnCommute.TblAccount.AsNoTracking()
                 .Where(p => (p.Email == email)
                 && p.Password == password)
                 .First();
        }
        public List<Account> GetAllAccounts()
        {
            return _dbConnectOnCommute.TblAccount.AsNoTracking().ToList();
        }
        public Account AddAccount(Account user)
        {
            //Unique values
            var email = user.Email;

            if (_dbConnectOnCommute.TblAccount.AsNoTracking().Where(e => e.Email == email).Any())
                throw new System.Exception("Username/Email already in use");

            _dbConnectOnCommute.TblAccount.Add(user);
            _dbConnectOnCommute.SaveChanges();
            return user;
        }
        public Account UpdateAccount(Account user)
        {
            _dbConnectOnCommute.Update(user);
            _dbConnectOnCommute.SaveChanges();
            return GetAccountById(user.Id);
        }
        public Account GetAccountById(int userId)
        {
            return _dbConnectOnCommute.TblAccount.AsNoTracking()
                .Where(p => p.Id == userId)
                .SingleOrDefault();
        }
        public Account GetUserByEmail(string email)
        {
            return _dbConnectOnCommute.TblAccount.AsNoTracking()
                .Where(p => p.Email == email)
                .SingleOrDefault();
        }

        public UserPosition AddUserPosition(UserPosition position)
        {
            
            _dbConnectOnCommute.TblPosition.Add(position);
            _dbConnectOnCommute.SaveChanges();
            return position;
        }
        public List<Account> GetPeopleNearUser(int userId)
        {
            var latestPosition = _dbConnectOnCommute.TblPosition
                .Where(p => p.AccountId == userId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefault();

            if (latestPosition == null)
                return new List<Account>();

            var time = 30;
            double meters = 100;
            double temp = 0;
            return _dbConnectOnCommute.TblPosition
                .Include(p => p.Account)
                .Where(p =>
                p.Account.FindableStatus == true
                && Math.Abs((DateTime.Now.ToUniversalTime() - p.Timestamp).TotalSeconds) <= time
                && (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)) <= meters
                && p.AccountId != userId)

                .OrderByDescending(p =>
                (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)))
                .Select(p => p.Account)
                .Distinct()
                .ToList();
        
        }
        public Account GetNearestPerson(int userId)
        {
            var latestPosition = _dbConnectOnCommute.TblPosition
                .Where(p => p.AccountId == userId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefault();

            if (latestPosition == null)
                return null;

            var time = 30;
            double meters = 100;
            return  _dbConnectOnCommute.TblPosition
                 .Where(p =>
                p.Account.FindableStatus == true
                && Math.Abs((DateTime.Now.ToUniversalTime() - p.Timestamp).TotalSeconds) <= time
                && (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)) <= meters
                && p.AccountId != userId)
                .Include(p => p.Account)
                .OrderByDescending(p =>
                (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)))
                .Select(p => p.Account)
                .Distinct()
                .FirstOrDefault();
        }
        public bool ConnectWithUser(int accountId,int targetId)
        {
            if (HasConnected(accountId, targetId))
                return false;

            var rec = _dbConnectOnCommute.TblConnection.Where(r =>
            (r.AccountId == accountId && r.TargetId == targetId))
                .FirstOrDefault();

            if (rec == null)
            {
                var con = new AccountConnection()
                {
                    AccountId = accountId,
                    TargetId = targetId,
                    Timestamp = DateTime.Now
                };
                _dbConnectOnCommute.TblConnection.Add(con);
                _dbConnectOnCommute.SaveChanges();
            }
            var val = HasConnected(accountId, targetId);
            if(val)
            {
                var user = GetAccountById(accountId);
                var target = GetAccountById(targetId);
                //Tell the other user
                EmitNotification(new AccountNotification()
                {
                    AccountId = targetId,
                    Type = 1,
                    Dismissed = false,
                    Timestamp = DateTime.Now.ToUniversalTime(),
                    Text = "Congratulations, you have a new connection! You and " + user.FirstName + " " + user.LastName + " are now connected!"
                });
                EmitNotification(new AccountNotification()
                {
                    AccountId = accountId,
                    Type = 1,
                    Dismissed = false,
                    Timestamp = DateTime.Now.ToUniversalTime(),
                    Text = "Congratulations, you have a new connection! You and " + target.FirstName + " " + target.LastName + " are now connected!"
                });
            }
            return val;

        }
        public bool HasConnected(int accountId,int targetId)
        {
            var a = _dbConnectOnCommute.TblConnection.Where(r =>
            (r.AccountId == accountId && r.TargetId == targetId))
                .FirstOrDefault();
            var b = _dbConnectOnCommute.TblConnection.Where(r =>
            (r.AccountId == targetId && r.TargetId == accountId))
                .FirstOrDefault();
            return b != null && a != null;
        }
        public AccountNotification EmitNotification(AccountNotification notification)
        {
            _dbConnectOnCommute.TblNotification.Add(notification);
            _dbConnectOnCommute.SaveChanges();
            return notification;
        }
        public List<AccountNotification> GetAvailableNotifications(int accountId)
        {
            var notifs = _dbConnectOnCommute.TblNotification.Where(n => n.AccountId == accountId && !n.Dismissed).ToList();
            notifs.ForEach(n => n.Dismissed = true);
            _dbConnectOnCommute.TblNotification.UpdateRange(notifs);
            _dbConnectOnCommute.SaveChanges();
            return notifs;
            
        }
        public List<AccountConnection> GetAccountConnections(int userId)
        {
            return _dbConnectOnCommute.TblConnection.Where(c => c.AccountId == userId)
                .Include(c => c.Target)
                .ToList();
        }
    }
}