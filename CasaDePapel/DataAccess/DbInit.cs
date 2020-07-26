using System.Linq;
using CasaDePapel.Domain;
using Microsoft.EntityFrameworkCore;

namespace CasaDePapel.DataAccess
{
    public class DbInit
    {
       public static void Initialize(BankContext context)
       {
           //context.Database.EnsureDeleted();
           context.Database.EnsureCreated();
           context.Database.Migrate();

            // Look for any students.
            if (context.Accounts.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User() {Name="user1"},
                new User() {Name="user2"},
                new User() {Name="user3"}
            };
            
            foreach (var u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var accounts = new Account[]
            {
                new Account() {Iban = "RO10BTRL", AccountType = BankAccountType.Current,Owner  = users[0], IsActive = true},
                new Account() {Iban = "RO11BTRL", AccountType = BankAccountType.Deposit, Owner = users[0], IsActive = true},
                new Account() {Iban = "RO20BTRL", AccountType = BankAccountType.Current,Owner = users[1], IsActive = true},
                new Account() {Iban = "RO30BTRL", AccountType = BankAccountType.Savings,Owner = users[2], IsActive = true},
            };
            foreach (var a in accounts)
            {
                context.Accounts.Add(a);
            }
            context.SaveChanges();
        }
    }
}