using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UI.Function.Main;
using UI.FunctionApp.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace UI.Function.Main
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
            
            services.SetupCosmosDb(_config);

        }
    }
}
