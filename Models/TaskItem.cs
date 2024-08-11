using System.Text.Json.Serialization;

namespace ToDoAppBackend.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public TaskMessage TaskMessage { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskState TaskState { get; set; } = TaskState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<TaskItem>? SubTasks { get; set; }
    }
    
    public class TaskMessage
    {
        public string? Message { get; set; } // Example: "This task can be completed after task {123} is completed."
        public ICollection<long> ReferencedTaskIds { get; set; } // Store only IDs
    }
}
