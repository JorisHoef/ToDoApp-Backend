using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ToDoAppBackend.Models
{
    public class TaskItemContext : DbContext
    {
        public TaskItemContext(DbContextOptions<TaskItemContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Convert enums to strings in the database
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.TaskDataState)
                .HasConversion(
                    v => v.ToString(),  // Convert enum to string for storage
                    v => (TaskDataState)Enum.Parse(typeof(TaskDataState), v)  // Convert string to enum
                );
        }

        public DbSet<TaskItem> TaskItems { get; set; } = null!;
    }
}
