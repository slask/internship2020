using System;
using System.Collections.Generic;
using System.Linq;
using CasaDePapel.Controllers;
using CasaDePapel.Controllers.Models;
using CasaDePapel.DataAccess;
using CasaDePapel.Domain;
using CasaDePapel.Infrastructure;
using CasaDePapel.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CasaDePapel.Application
{
    public class BankAccountApplicationService
    {
        private readonly BankContext _db;
        private readonly NotificationService _notificationService;
        private readonly CurrencyRateService _currencyRateService;
        private readonly IConfiguration _configuration;

        public BankAccountApplicationService(BankContext db, NotificationService notificationService, CurrencyRateService currencyRateService, IConfiguration configuration)
        {
            _configuration = configuration;
            _currencyRateService = currencyRateService;
            _notificationService = notificationService;
            _db = db;
        }

        public void Deposit(DepositAmountModel model)
        {
            //TODO: - showcase simple state based testing on domain model

            //get the account
            var account = _db.Accounts.Find(model.AccountId);

            // use deposit method
            account.Deposit(model.Amount);

            //save the account to DB
            _db.SaveChanges();

        }

        public void Withdraw(WithdrawAmountModel model)
        {
            //TODO: - showcase state based testing + mock verification
            
            //get the account
            var account = _db.Accounts.Find(model.Id);

            // take money out
            if (account.Balance >= model.Amount)
            {
                account.Balance -= model.Amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            //save the account to DB
            _db.SaveChanges();
            
            //call notifyService with the event
            _notificationService.MoneyWithdrawn(account.Iban, account.Balance);
        }

        public object Exchange(ExchangeModel model)
        {
            //TODO: - showcase the need for a stub for the exchange rate service
            //get the account
            var account = _db.Accounts.FirstOrDefault(a => a.Iban == model.AccountNr);
            //get the currency as string

            //pass currency + target currency to rate service
            decimal rate = _currencyRateService.GetRate(account.Currency, model.TargetCurrency);

            //after get rate do the conversion
            decimal convertedAmount = model.Amount * rate;

            //create a new account on the fly in the new currency if it does not exist (it will not)
            var targetAccount = _db.Accounts.FirstOrDefault(a => a.Currency == model.TargetCurrency &&
                                                                 a.OwnerId == model.UserId);
            if (targetAccount == null)
            {
                var newAccount = new Account
                {
                    Balance = 0,
                    Currency = model.TargetCurrency,
                    Iban = "RO100BTRL123XX",
                    AccountType = BankAccountType.Current,
                    IsActive = true,
                    OwnerId = account.OwnerId,
                };
                newAccount.Deposit(convertedAmount);
                _db.Accounts.Add(newAccount);
            }
            else
            {
                targetAccount.Deposit(convertedAmount);
            }

            //deduct old amount from current account
            if (account.Balance >= model.Amount)
            {
                account.Balance -= model.Amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            //save all
            _db.SaveChanges();
            return new BankAccountOutputModel
            {
                Amount = targetAccount.Balance,
                Currency = targetAccount.Currency,
                AccountNr = targetAccount.Iban,
                Type = targetAccount.AccountType.ToString()
            };
        }

        public void Transfer(TransferMoneyModel model)
        {
            //TODO: - show refactoring strength when having UTs (->domsrv)
           
            //2% commission on transfers
            var from = _db.Accounts.FirstOrDefault(a => a.Iban == model.IbanFrom);
            var to = _db.Accounts.FirstOrDefault(a => a.Iban == model.IbanTo);

            //calculate commission
            decimal fee = model.Amount * 0.02m;
            
            // take money out from source
            if (from.Balance >= model.Amount+fee)
            {
                from.Balance -= model.Amount+fee;
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds!");
            }
            
            to.Deposit(model.Amount);

            _db.SaveChanges();
        }

        public List<BankAccountOutputModel> ListActiveBankAccounts(int userId)
        {
            //TODO: - make sure there is data in DB and showcase testing an ef query with inMemoryDB
            var allActive = _db.Accounts
                .Where(a => a.IsActive && a.OwnerId == userId)
                .Select(a =>
                new BankAccountOutputModel()
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
            List<Account> allAccounts;
            using (SqlConnection connection = new SqlConnection(
                _configuration.GetConnectionString("BankContext")))
            {
                connection.Open();
                allAccounts = connection.Query<Account>("SELECT * FROM [Accounts]").ToList();
            }

            // sum all ammounts and return
            return allAccounts.Sum(a => a.Balance);
        }
    }
}