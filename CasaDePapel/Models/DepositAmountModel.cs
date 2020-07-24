
namespace CasaDePapel.Controllers.Models
{
    public class DepositAmountModel
    {
        public decimal Amount { get; set; }
        public string Iban { get; set; }
        public int UserId { get; set; }
    }
}