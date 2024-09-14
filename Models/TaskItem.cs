using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ToDoAppBackend.Models
{
    public class TaskItem
    {
        [Key]
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("taskItemMessage")]
        public TaskItemMessage? TaskItemMessage { get; set; }
        
        [JsonProperty("taskDataState")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskDataState TaskDataState { get; set; } = TaskDataState.OPEN;
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        [JsonProperty("completedAt")]
        public DateTime CompletedAt { get; set; }
        
        [JsonProperty("deadlineAt")]
        public DateTime DeadlineAt { get; set; }
        
        [JsonProperty("subTasks")]
        public ICollection<TaskItem>? SubTasks { get; set; } = null;
        
        [JsonProperty("parentTaskId")]
        public long? ParentTaskId { get; set; }
    }
}