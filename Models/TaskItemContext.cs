using Microsoft.EntityFrameworkCore;

namespace ToDoAppBackend.Models
{
    public class TaskItemContext : DbContext
    {
        public TaskItemContext(DbContextOptions<TaskItemContext> options)
                : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; } = null!;
    }
}