using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoAppBackend.Models
{
    public class TaskItem
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public TaskItemMessage? TaskItemMessage { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskDataState TaskDataState { get; set; } = TaskDataState.OPEN;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<TaskItem>? SubTasks { get; set; } = null;
    }
}
