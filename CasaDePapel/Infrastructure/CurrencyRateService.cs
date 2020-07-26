using System.Threading;

namespace CasaDePapel.Infrastructure
{
    public class CurrencyRateService
    {
        public decimal GetRate(string fromCurrency, string toCurrency)
        {
            // something slow from a 3rd party
            Thread.Sleep(1000);
            return 1.1m;
        }
    }
}