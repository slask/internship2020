namespace CasaDePapel.Controllers
{
    public class ExchangeModel
    {
        public string AccountNr { get; set; }
        public int UserId { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}