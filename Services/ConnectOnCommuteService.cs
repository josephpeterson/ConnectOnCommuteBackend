using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using ConnectOnCommuteBackend.DataAccess;
using ConnectOnCommuteBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConnectOnCommuteBackend.Services
{
    public interface IConnectOnCommuteService
    {
         
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
    }
}
