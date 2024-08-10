using Microsoft.EntityFrameworkCore;

namespace ToDoAppBackend.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
                : base(options)
        {
        }

        public DbSet<TaskItem> TodoItems { get; set; } = null!;
    }
}