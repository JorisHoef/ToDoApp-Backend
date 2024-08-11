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

    /// <summary>
    /// This relation aims to connect a task with other tasks that are being referenced in the Task itself
    /// </summary>
    public class TaskRelation
    {
        public TaskItem TaskItem { get; set; }
        public ICollection<TaskItem> ReferencedTaskList { get; set; }
    }

    public class TaskMessage
    {
        public string? Message { get; set; } //Example string: "This task can be completed after task {x.Id} is completed". Where task {x} references a task that already exists by Id
        public ICollection<TaskItem> TaskItem { get; set; }
    }
}
