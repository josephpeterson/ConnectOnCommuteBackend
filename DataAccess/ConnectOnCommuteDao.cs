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
        Account GetAccountById(int userId);
        Account GetAccountByLogin(string email, string password);
        List<Account> GetAllAccounts();
        Account GetNearestPerson(int userId);
        List<Account> GetPeopleNearUser(int userId);
        Account GetUserByEmail(string email);
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
                .Where(p =>
                Math.Abs((DateTime.Now.ToUniversalTime() - p.Timestamp).TotalSeconds) <= time
                && (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)) <= meters
                && p.AccountId != userId)
                .Include(p => p.Account)
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
            var acc = _dbConnectOnCommute.TblPosition
                .Where(p =>
                Math.Abs((DateTime.Now.ToUniversalTime() - p.Timestamp).TotalSeconds) <= time
                && (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)) <= meters
                && p.AccountId != userId)
                .Include(p => p.Account)
                .OrderByDescending(p =>
                (new GeoCoordinate(latestPosition.Latitude, latestPosition.Longitude)).GetDistanceTo(new GeoCoordinate(p.Latitude, p.Longitude)))
                .Select(p => p.Account)
                .Distinct()
                .FirstOrDefault();
            return acc;
        }
    }
}