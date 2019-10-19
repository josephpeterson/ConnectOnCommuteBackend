using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ConnectOnCommuteBackend.Services;
using ConnectOnCommuteBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace ConnectOnCommuteBackend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        public readonly IConnectOnCommuteService _connectOnCommuteService;
        private readonly IAccountService _accountService;
        public readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration config, IAccountService accountService,IConnectOnCommuteService connectOnCommuteService, ILogger<AccountController> logger)
        {
            _config = config;
            _connectOnCommuteService = connectOnCommuteService;
            _accountService = accountService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        [Route("/v1/Account/{id}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult GetDetailedAccount(int id)
        {
            try
            {
                int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var data = _accountService.GetUserById(userId);
                return new OkObjectResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET /v1/Account/{id} endpoint caught exception: { ex.Message } Details: { ex.ToString() }");
                return NotFound();
            }
        }
        [Authorize]
        [HttpGet]
        [Route("/v1/Account/Current")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult GetCurrentUser()
        {
            try
            {
                int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var data = _accountService.GetUserById(userId);
                return new OkObjectResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET /v1/Account/Current endpoint caught exception: { ex.Message } Details: { ex.ToString() }");
                return NotFound();
            }
        }
        [HttpPost]
        [Route("/v1/Account/Signup")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult CreateNewAccount([FromBody] SignupRequest signupRequest)
        {
            try
            {
                var account = _accountService.AddUser(signupRequest);
                var login = _accountService.LoginUser(account.Email, account.Password);
                return new OkObjectResult(login);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET /v1/Account/Signup endpoint caught exception: { ex.Message } Details: { ex.ToString() }");
                return NotFound();
            }
        }
        [HttpPost]
        [Route("/v1/Account/Login")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult LoginAccount([FromBody] SignupRequest signupRequest)
        {
            try
            {
                var account = _accountService.AddUser(signupRequest);
                var login = _accountService.LoginUser(account.Email, account.Password);
                return new OkObjectResult(login);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GET /v1/Account/Signup endpoint caught exception: { ex.Message } Details: { ex.ToString() }");
                return NotFound();
            }
        }
    }

}
