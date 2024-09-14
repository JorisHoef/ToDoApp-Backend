using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ToDoAppBackend.Models;

namespace ToDoAppBackend.Controllers
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskItemContext>
    {
        public TaskItemContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskItemContext>();

            // Retrieve and validate the database connection string from environment variables
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The DATABASE_CONNECTION environment variable is not set.");
            }

            optionsBuilder.UseNpgsql(connectionString);

            return new TaskItemContext(optionsBuilder.Options);
        }
    }
}