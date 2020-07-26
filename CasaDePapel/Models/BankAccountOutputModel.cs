namespace CasaDePapel.Models
{
    public class BankAccountOutputModel
    {
        public string AccountNr { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
    }
}