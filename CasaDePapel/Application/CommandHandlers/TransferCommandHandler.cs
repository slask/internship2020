using System.Linq;
using CasaDePapel.Controllers;
using CasaDePapel.DataAccess;
using CasaDePapel.Domain.Services;

namespace CasaDePapel.Application.CommandHandlers
{
    public class TransferCommandHandler
    {
        private readonly BankContext _db;

        public TransferCommandHandler(BankContext db)
        {
            _db = db;
        }
        
        public void Transfer(TransferMoneyModel model)
        {
            //TODO: - show refactoring strength when having UTs (->domsrv)
           
            var from = _db.Accounts.FirstOrDefault(a => a.Iban == model.IbanFrom);
            var to = _db.Accounts.FirstOrDefault(a => a.Iban == model.IbanTo);

            var transferService = new TransferService();
            transferService.Transfer(from, to, model.Amount);

            _db.SaveChanges();
        }
    }
}