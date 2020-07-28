using System.Threading;

namespace CasaDePapel.Infrastructure
{
    public class CurrencyRateService
    {
        public virtual decimal GetRate(string fromCurrency, string toCurrency)
        {
            // something slow from a 3rd party
            Thread.Sleep(1000);
            return 5.5m;
        }
    }
}