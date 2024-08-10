using System.Text.Json.Serialization;

namespace ToDoApp_Backend.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskState TaskState { get; set; } = TaskState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<TodoItem>? SubTasks { get; set; }
    }
}
