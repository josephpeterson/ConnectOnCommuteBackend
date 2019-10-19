using System;
using System.Collections.Generic;
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
        Account GetAccountById(int userId);
        Account GetAccountByLogin(string email, string password);
        List<Account> GetAllAccounts();
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
    }
}