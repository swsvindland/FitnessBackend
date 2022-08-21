using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FitnessApi.Startup))]

namespace FitnessApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // builder.Services.AddHttpClient();

            // builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}