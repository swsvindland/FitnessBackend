using FitnessRepository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FitnessApi.Startup))]

namespace FitnessApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>(); 

            builder.Services.AddHttpClient();
            
            builder.Services.AddDbContext<FitnessContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<ISupplementRepository, SupplementRepository>();

        }
    }
}