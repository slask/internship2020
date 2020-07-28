using System.Linq;
using CasaDePapel.Controllers;
using CasaDePapel.DataAccess;
using CasaDePapel.Domain;
using CasaDePapel.Infrastructure;
using CasaDePapel.Models;

namespace CasaDePapel.Application.CommandHandlers
{
    public class ExchangeCommandHandler
    {
        private readonly BankContext _db;
        private readonly CurrencyRateService _currencyRateService;

        public ExchangeCommandHandler(BankContext db, CurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
            _db = db;
        }
        
        public BankAccountOutputModel Exchange(ExchangeModel model)
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
                targetAccount = new Account
                {
                    Balance = 0,
                    Currency = model.TargetCurrency,
                    Iban = "RO100BTRL123XX",
                    AccountType = BankAccountType.Current,
                    IsActive = true,
                    OwnerId = account.OwnerId,
                };
                targetAccount.Deposit(convertedAmount);
                _db.Accounts.Add(targetAccount);
            }
            else
            {
                targetAccount.Deposit(convertedAmount);
            }

            //deduct old amount from current account
            account.Withdraw(model.Amount);

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
    }
}