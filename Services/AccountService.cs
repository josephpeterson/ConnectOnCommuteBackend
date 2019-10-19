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
    public interface IAccountService
    {
         Account AddUser(SignupRequest user);
         List<Account> GetAllUsers();
         Account GetUserById(int userId);
         Account GetUserByEmail(string email);
         Account UpdateUser(Account user);
         void DeleteUser(int userId);
         string LoginUser(string email, string password);
    }
    public class AccountService: IAccountService
    {
        private IConfiguration _configuration;
        private IConnectOnCommuteDao _connectOnCommuteDao;
        private readonly IHttpContextAccessor _context;

        public AccountService(/*IHttpContextAccessor context,*/ IConfiguration configuration, IConnectOnCommuteDao connectOnCommuteDao)
        {
            //_mapper = mapper;
            //_context = context;
            _configuration = configuration;
            _connectOnCommuteDao = connectOnCommuteDao;
        }
        /*
        public int GetCurrentUserId()
        {
            return Convert.ToInt16(_context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        */
        public Account AddUser(SignupRequest user)
        {
            var account = new Account()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Question = user.Question
            };
            account.Role = "User";
            account.SeniorityDate = DateTime.Now;
            return _connectOnCommuteDao.AddAccount(account);
        }
        public List<Account> GetAllUsers()
        {
            return _connectOnCommuteDao.GetAllAccounts();
        }
        public Account GetUserById(int userId)
        {
            return _connectOnCommuteDao.GetAccountById(userId);
        }
        public Account GetUserByEmail(string email)
        {
            return _connectOnCommuteDao.GetUserByEmail(email);
        }
        public Account UpdateUser(Account user)
        {
            throw new NotImplementedException();
        }
        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }
        public string LoginUser(string email, string password)
        {
            Account user = _connectOnCommuteDao.GetAccountByLogin(email, password);


            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:LengthMins"])),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}
