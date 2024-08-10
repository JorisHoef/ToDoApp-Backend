using System.Text.Json.Serialization;

namespace ToDoApp_Backend.Models
{
    /// <summary>
    /// Data Transfer Object for TodoItem
    /// </summary>
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskState TaskState { get; set; } = TaskState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}