using CasaDePapel.Controllers.Models;
using CasaDePapel.DataAccess;

namespace CasaDePapel.Application.CommandHandlers
{
    public class DepositCommandHandler
    {
        private readonly BankContext _db;

        public DepositCommandHandler(BankContext db)
        {
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
    }
}