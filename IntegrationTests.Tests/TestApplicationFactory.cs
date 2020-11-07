using System;
using System.IO;
using IntegrationTests.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Tests
{
    public class TestApplicationFactory : WebApplicationFactory<Startup>
    {
        public TestDatabase TestDatabase { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                // add custom configuration
                var path = AppDomain.CurrentDomain.BaseDirectory;
                configurationBuilder.AddJsonFile(Path.Combine(path, "appsettings.Test.json"));
            });

            builder.ConfigureServices((context, collection) =>
            {
                // read from the configuration
                var connectionString = context.Configuration.GetConnectionString("Local");
                // initialise the DB
                TestDatabase = new TestDatabase(connectionString);
                TestDatabase.Initialise();
            });
        }
    }
}
