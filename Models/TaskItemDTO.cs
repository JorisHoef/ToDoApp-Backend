using System.Text.Json.Serialization;

namespace ToDoAppBackend.Models
{
    /// <summary>
    /// Data Transfer Object for TodoItem
    /// </summary>
    public class TaskItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? TaskMessage { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskState TaskState { get; set; } = TaskState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<TaskItem>? SubTasks { get; set; }
    }
}