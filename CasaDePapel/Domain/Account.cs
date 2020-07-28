
using System;

namespace CasaDePapel.Domain
{
    public class Account
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public virtual User Owner { get; set; }
        
        public int OwnerId { get; set; }
        public string Iban { get; set; }
        public BankAccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException("Amount must be >= 0");
            }

            Balance += amount;
        }
        
        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds!");
            }
        }
        
        
    }
}