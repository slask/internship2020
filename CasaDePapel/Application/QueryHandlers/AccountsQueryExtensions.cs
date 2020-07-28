using System.ComponentModel.Design;
using System.Linq;
using CasaDePapel.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CasaDePapel.Application.QueryHandlers
{
    public static class AccountsQueryExtensions
    {
        public static IQueryable<Account> Active(this IQueryable<Account> query)
        {
            query = query.Where(x => x.IsActive);
            return query;
        }

        public static IQueryable<Account> ForOwner(this IQueryable<Account> query, int ownerId)
        {
            return query.Where(x => x.OwnerId == ownerId);
        }
    }
}