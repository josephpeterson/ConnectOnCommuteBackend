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
        List<Account> GetPeopleNearUser(int userId);
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
            var pos = new UserPosition()
            {
                AccountId = userId,
                Timestamp = DateTime.Now.ToUniversalTime(),
                Longitude = userLocation.Longitude,
                Latitude = userLocation.Latitude
            };
            return _connectOnCommuteDao.AddUserPosition(pos);
        }
        public List<Account> GetPeopleNearUser(int userId)
        {
            List<Account> accounts = _connectOnCommuteDao.GetPeopleNearUser(userId);
            return accounts;
        }
    }
}
