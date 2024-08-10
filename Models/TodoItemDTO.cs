namespace ToDoApp_Backend.Models
{
    /// <summary>
    /// Data Transfer Object for TodoItem
    /// </summary>
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}