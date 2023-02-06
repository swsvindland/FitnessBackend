using System;
using System.Text.Json.Serialization;
using FitnessRepository;
using FitnessRepository.Repositories;
using FitnessServices.Services;
using FoodApi;
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

            builder.Services.AddHttpClient<IFatSecretApi, FatSecretApi>();

            builder.Services.AddMvcCore().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // init db
            builder.Services.AddDbContext<FitnessContext>(options =>
                options.UseMySql(connectionString, serverVersion));

            // add db calls
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISupplementRepository, SupplementRepository>();
            builder.Services.AddScoped<IBodyRepository, BodyRepository>();
            builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            builder.Services.AddScoped<IFoodRepository, FoodRepository>();

            // add external api calls
            builder.Services.AddScoped<IFatSecretApi, FatSecretApi>();

            // add service functions
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISupplementService, SupplementService>();
            builder.Services.AddScoped<IBodyService, BodyService>();
            builder.Services.AddScoped<IFoodService, FoodService>();
            builder.Services.AddScoped<IWorkoutService, WorkoutService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}