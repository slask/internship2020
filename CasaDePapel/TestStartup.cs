using CasaDePapel.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDePapel.Tests
{
    public class TestStartup: Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void SetupDb(IServiceCollection services)
        {
            services.AddDbContext<BankContext>(o => o.UseInMemoryDatabase("TestDBFromStartup"));
        }
    }
}