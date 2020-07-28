using System;
using System.Net.Http;

namespace CasaDePapel.Infrastructure
{
    public interface INotificationService
    {
        void MoneyWithdrawn(string iban, decimal moneyLeft);
    }

    public class NotificationService : INotificationService
    {
        public NotificationService()
        {
           
        }
        public void MoneyWithdrawn(string iban, decimal moneyLeft)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync("http://www.google.com").Result;
            if(!result.IsSuccessStatusCode)
                throw new Exception("Oops notification not sent");
        }
    }
}