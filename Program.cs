using Microsoft.EntityFrameworkCore;
using ToDoAppBackend.Models;
using ToDoAppBackend.Services;

namespace ToDoAppBackend
{
    public class Program
    {
        private const string FALLBACK_ADDRESS = "http://localhost/";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiBaseUrl = Environment.GetEnvironmentVariable("API_SERVER") ?? $"{FALLBACK_ADDRESS}";
            var apiUri = new Uri(apiBaseUrl);

            ConfigureServices(builder.Services, apiUri);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            Configure(app, builder.Environment);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, Uri apiUri)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });

            AddDbContexts(services);

            services.AddTransient<LinkCreator>();
            services.AddScoped<ITaskItemMessageResolver, TaskItemMessageResolver>();

            // Configure API client
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = apiUri;
            });

            services.AddSingleton(apiUri);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void AddDbContexts(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

            if (environment == "Local")
            {
                // Use In-Memory Database in Local mode
                services.AddDbContext<TaskItemContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            }
            else
            {
                // Use PostgreSQL in other environments
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("DATABASE_CONNECTION environment variable is not set.");
                }

                services.AddDbContext<TaskItemContext>(opt => opt.UseNpgsql(connectionString));
            }
        }

        public static void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Redirect root URL to Swagger
                app.MapGet("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
