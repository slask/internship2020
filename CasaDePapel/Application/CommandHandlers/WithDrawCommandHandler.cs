using CasaDePapel.Controllers;
using CasaDePapel.Controllers.Models;
using CasaDePapel.DataAccess;
using CasaDePapel.Infrastructure;

namespace CasaDePapel.Application.CommandHandlers
{
    public class WithDrawCommandHandler
    {
        private readonly BankContext _db;
        private readonly INotificationService _notificationService;

        public WithDrawCommandHandler(BankContext db, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _db = db;
        }
        
        public void Withdraw(WithdrawAmountModel model)
        {
            //TODO: - showcase state based testing + mock verification
            
            //get the account
            var account = _db.Accounts.Find(model.Id);

            // take money out
            account.Withdraw(model.Amount);

            //save the account to DB
            _db.SaveChanges();
            
            //call notifyService with the event
            _notificationService.MoneyWithdrawn(account.Iban, account.Balance);
        }
    }
}