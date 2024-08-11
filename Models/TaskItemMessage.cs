using System.ComponentModel.DataAnnotations;

namespace ToDoAppBackend.Models
{
    public class TaskItemMessage
    {
        [Key]
        public long Id { get; set; }
        public string? Message { get; set; }
        public IList<long>? ReferencedTaskIds { get; set; }
    }
}