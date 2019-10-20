using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConnectOnCommuteBackend.DataAccess;
using ConnectOnCommuteBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConnectOnCommuteBackend.Services
{
    public interface IConnectOnCommuteService
    {
        UserPosition AddUserPosition(int userId, UserCoords userLocation);
        bool ConnectWithUser(int accountId, int targetId);
        AccountNotification EmitNotification(AccountNotification notification);
        List<AccountConnection> GetAccountConnections(int userId);
        List<AccountNotification> GetAvailableNotifications(int accountId);
        Account GetNearestPerson(int userId);
        List<Account> GetPeopleNearUser(int userId);
        bool HasConnection(int accountId, int targetId);
        Account UpdateAccount(Account user);
    }
    public class ConnectOnCommuteService: IConnectOnCommuteService
    {
        private IConfiguration _configuration;
        private IConnectOnCommuteDao _connectOnCommuteDao;
        //private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;

        public ConnectOnCommuteService(IConfiguration configuration, IConnectOnCommuteDao connectOnCommuteDao)
        {
            //_context = context;
            _configuration = configuration;
            _connectOnCommuteDao = connectOnCommuteDao;
        }

        public  UserPosition AddUserPosition(int userId,UserCoords userLocation)
        {
            var acc = _connectOnCommuteDao.GetAccountById(userId);
            if (!acc.FindableStatus)
                return null;
            var pos = new UserPosition()
            {
                AccountId = userId,
                Timestamp = DateTime.Now.ToUniversalTime(),
                LongitudeStr = userLocation.Longitude.ToString(),
                LatitudeStr = userLocation.Latitude.ToString()
            };
            return _connectOnCommuteDao.AddUserPosition(pos);
        }
        public List<Account> GetPeopleNearUser(int userId)
        {
            var acc = _connectOnCommuteDao.GetAccountById(userId);
            if (!acc.FindableStatus)
                return null;
            List<Account> accounts = _connectOnCommuteDao.GetPeopleNearUser(userId);
            return accounts;
        }
        public Account GetNearestPerson(int userId)
        {
            var acc = _connectOnCommuteDao.GetAccountById(userId);
            if (!acc.FindableStatus)
                return null;
            return _connectOnCommuteDao.GetNearestPerson(userId);
        }
        public bool ConnectWithUser(int accountId,int targetId)
        {
            return _connectOnCommuteDao.ConnectWithUser(accountId, targetId);
        }
        public bool HasConnection(int accountId,int targetId)
        {
            return _connectOnCommuteDao.HasConnected(accountId, targetId);
        }
        public AccountNotification EmitNotification(AccountNotification notification)
        {
            return _connectOnCommuteDao.EmitNotification(notification);
        }
        public List<AccountNotification> GetAvailableNotifications(int accountId)
        {
            return _connectOnCommuteDao.GetAvailableNotifications(accountId);
        }
        public List<AccountConnection> GetAccountConnections(int userId)
        {
            var con = _connectOnCommuteDao.GetAccountConnections(userId);
            return con;
        }
        public Account UpdateAccount(Account user)
        {
            return _connectOnCommuteDao.UpdateAccount(user);
        }
    }
}
