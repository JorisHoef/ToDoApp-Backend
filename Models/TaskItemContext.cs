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
                        .HasConversion(new EnumToStringConverter<TaskDataState>());
        }

        public DbSet<TaskItem> TaskItems { get; set; } = null!;
    }
}