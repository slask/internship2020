namespace CasaDePapel.Controllers
{
    public class BankAccountOutputModel
    {
        public string AccountNr { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public BankAccountType Type { get; set; }
        public string Currency { get; set; }
    }
}