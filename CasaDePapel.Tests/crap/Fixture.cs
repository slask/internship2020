using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using CasaDePapel.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CasaDePapel.Tests
{
    public class Fixture2
    {
        //private ServiceCollection _services;
        //private readonly ServiceProvider _provider;
        private readonly IHost _host;

        public Fixture2()
        {
            //var config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>()).Build();
            
            //_services = new ServiceCollection();
            //var startup = new TestStartup(config);
            //startup.ConfigureServices(_services);
            //_provider = _services.BuildServiceProvider();
            _host = Host.CreateDefaultBuilder().ConfigureWebHost(b => b.UseStartup<TestStartup>()).Build();
            
        }
        
        public static BankContext GetNewInMemoryDatabase(string dbName)
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;
            var context = new BankContext(options);
            return context;
        }

        public T GetSut<T>() where T : class
        {
            using var scope = _host.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}