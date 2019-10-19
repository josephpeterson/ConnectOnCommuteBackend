﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectOnCommuteBackend.Models;
using ConnectOnCommuteBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConnectOnCommuteBackend.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private IConfiguration _config;
        private IConnectOnCommuteService _connectOnCommuteService;
        private IAccountService _accountService;
        private ILogger<PeopleController> _logger;

        public PeopleController(IConfiguration config, IAccountService accountService, IConnectOnCommuteService connectOnCommuteService, ILogger<PeopleController> logger)
        {
            _config = config;
            _connectOnCommuteService = connectOnCommuteService;
            _accountService = accountService;
            _logger = logger;
        }
        [HttpPost]
        [Route("People/UpdateLocation")]
        public IActionResult SendUserLocation([FromBody] UserCoords location)
        {
            int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var pos = _connectOnCommuteService.AddUserPosition(userId,location);
            return new OkObjectResult(pos);
        }
        [HttpGet]
        [Route("People/Near")]
        public IActionResult GetPeopleNearUser()
        {
            int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var accounts = _connectOnCommuteService.GetPeopleNearUser(userId);
            return new OkObjectResult(accounts);
        }
        [HttpGet]
        [Route("People/Nearest")]
        public IActionResult GetNearestPerson()
        {
            int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var account = _connectOnCommuteService.GetNearestPerson(userId);
            return new OkObjectResult(account);
        }
    }
}
