using System;
using Data.CosmosDb;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UI.FunctionApp;
using UI.FunctionApp.Data;
using UI.FunctionApp.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace UI.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _config;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();

            _config = new ConfigurationBuilder()
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var entityDataStoreOptions = new RepositoryOptions();
            
            services.SetupCosmosDb(_config);

            services.AddSingleton(entityDataStoreOptions);

            services.AddTransient<IToDoRepository, ToDoRepository>();
            services.AddTransient<IToDoCommentChildRepository, ToDoCommentChildRepository>();
        }

    }
}
