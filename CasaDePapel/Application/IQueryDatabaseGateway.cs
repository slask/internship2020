using System.Collections.Generic;
using System.Linq;
using CasaDePapel.Domain;

namespace CasaDePapel.Application
{
    public interface IQueryDatabaseGateway
    {
        IQueryable<Account> Accounts { get; }
        List<Account> GetAllAccounts();
    }
}