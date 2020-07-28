using System;
using System.Collections.Generic;
using System.Linq;
using CasaDePapel.Application.CommandHandlers;
using CasaDePapel.Application.QueryHandlers;
using CasaDePapel.Controllers;
using CasaDePapel.Controllers.Models;
using CasaDePapel.DataAccess;
using CasaDePapel.Domain;
using CasaDePapel.Domain.Services;
using CasaDePapel.Infrastructure;
using CasaDePapel.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CasaDePapel.Application
{
    public class BankAccountApplicationService
    {
        private readonly BankContext _db;
        private readonly INotificationService _notificationService;
        private readonly CurrencyRateService _currencyRateService;
        private readonly IConfiguration _configuration;
        private readonly IQueryDatabaseGateway _queryDatabaseGateway;

        public BankAccountApplicationService(BankContext db, INotificationService notificationService,
            CurrencyRateService currencyRateService, IConfiguration configuration, IQueryDatabaseGateway queryDatabaseGateway)
        {
            _queryDatabaseGateway = queryDatabaseGateway;
            _configuration = configuration;
            _currencyRateService = currencyRateService;
            _notificationService = notificationService;
            _db = db;
        }

        //commands
        
        public void Deposit(DepositAmountModel model)
        {
            new DepositCommandHandler(_db).Deposit(model);
        }

        public void Withdraw(WithdrawAmountModel model)
        {
            new WithDrawCommandHandler(_db, _notificationService).Withdraw(model);
        }

        public BankAccountOutputModel Exchange(ExchangeModel model)
        {
            var handler = new ExchangeCommandHandler(_db, _currencyRateService);
            return handler.Exchange(model);
        }

        public void Transfer(TransferMoneyModel model)
        {
            new TransferCommandHandler(_db).Transfer(model);
        }
        
        //QUERY
        
        public List<BankAccountOutputModel> ListActiveBankAccounts(int userId)
        {
            //TODO: - make sure there is data in DB and showcase testing an ef query with inMemoryDB

            var allActive = _queryDatabaseGateway.Accounts
                .Active()
                .ForOwner(userId)
                .Select(a =>
                new BankAccountOutputModel
                {
                    Amount = a.Balance,
                    Currency = a.Currency,
                    Type = a.AccountType.ToString(),
                    AccountNr = a.Iban,
                }).ToList();
            return allActive;
        }

        public decimal AdminGetBankTotalWorth()
        {
            //TODO: - showcase how to make the untestable testable and mock deps

            // get all accounts with dapper for perf
            var allAccounts = _queryDatabaseGateway.GetAllAccounts();

            // sum all ammounts and return
            return allAccounts.Sum(a => a.Balance);
        }
    }
}