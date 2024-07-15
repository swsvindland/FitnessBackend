using FitnessRepository;
using FitnessRepository.Repositories;
using FitnessServices.Services;
using FoodApi;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddHttpClient<IFatSecretApi, FatSecretApi>();

        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") ?? string.Empty;
        // init db
        services.AddDbContext<FitnessContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("FitnessApi")));

        // add db calls
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISupplementRepository, SupplementRepository>();
        services.AddScoped<IBodyRepository, BodyRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IFoodRepository, FoodRepository>();

        // add external api calls
        services.AddScoped<IFatSecretApi, FatSecretApi>();

        // add service functions
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISupplementService, SupplementService>();
        services.AddScoped<IBodyService, BodyService>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IDashboardService, DashboardService>();
    })
    .Build();

host.Run();