using Microsoft.EntityFrameworkCore;
using ToDoAppBackend.Models;
using ToDoAppBackend.Services;

namespace ToDoAppBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Access BaseUri from configuration
            var baseUri = builder.Configuration["BaseUri"];

            ConfigureServices(builder.Services, baseUri);
            
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            Configure(app, builder.Environment);
            
            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, string baseUri)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });
    
            AddDbContexts(services);
    
            services.AddTransient<LinkCreator>();
            services.AddScoped<ITaskItemMessageResolver, TaskItemMessageResolver>();

            // Read API_SERVER from environment variables
            var apiBaseUrl = Environment.GetEnvironmentVariable("API_SERVER") ?? "http://localhosthenkie";
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            services.AddSingleton(new Uri(baseUri)); // Add baseUri to application
    
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        
        private static void AddDbContexts(IServiceCollection services)
        {
            services.AddDbContext<TaskItemContext>(opt => opt.UseInMemoryDatabase("TodoList"));
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