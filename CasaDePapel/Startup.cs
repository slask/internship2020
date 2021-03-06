using System.Collections.Generic;
using CasaDePapel.Application;
using CasaDePapel.DataAccess;
using CasaDePapel.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CasaDePapel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<CurrencyRateService>();
            services.AddScoped<BankAccountApplicationService>();
            SetupDb(services);
        }

        protected virtual void SetupDb(IServiceCollection services)
        {
            services.AddDbContext<BankContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BankContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}