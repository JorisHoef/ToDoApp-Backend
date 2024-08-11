using ToDoAppBackend.Models;

namespace ToDoAppBackend.Services
{
    public interface ITaskItemMessageResolver
    {
        public string? ResolveTaskMessage(TaskItemMessage taskItemMessage);
    }
}