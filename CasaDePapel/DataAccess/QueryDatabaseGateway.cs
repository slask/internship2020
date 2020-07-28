using System.Collections.Generic;
using System.Linq;
using CasaDePapel.Application;
using CasaDePapel.Domain;
using CasaDePapel.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CasaDePapel.DataAccess
{
    public class QueryDatabaseGateway : IQueryDatabaseGateway
    {
        private readonly IConfiguration _configuration;
        private readonly BankContext _db;

        public QueryDatabaseGateway(IConfiguration configuration, BankContext db)
        {
            _db = db;
            _configuration = configuration;
        }



        public IQueryable<Account> Accounts => _db.Accounts.AsNoTracking();

        public List< Account> GetAllAccounts()
        {
            List<Account> allAccounts;
            using (SqlConnection connection = new SqlConnection(
                _configuration.GetConnectionString("BankContext")))
            {
                connection.Open();
                allAccounts = connection.Query<Account>("SELECT * FROM [Accounts]").ToList();
            }

            return allAccounts;
        }
    }
}