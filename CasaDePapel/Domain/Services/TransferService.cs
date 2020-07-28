namespace CasaDePapel.Domain.Services
{
    public class TransferService
    {
        public void Transfer(Account from, Account to, decimal amount)
        {
            decimal fee = amount * 0.02m;
            from.Withdraw(amount+fee);
            to.Deposit(amount);
        }
    }
}