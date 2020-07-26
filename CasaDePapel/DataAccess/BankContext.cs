using CasaDePapel.Domain;
using Microsoft.EntityFrameworkCore;

namespace CasaDePapel.DataAccess
{
    public class BankContext : DbContext
    {
        public BankContext (DbContextOptions<BankContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}