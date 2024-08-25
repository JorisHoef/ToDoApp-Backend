using ToDoAppBackend.Models;

namespace ToDoAppBackend.Services
{
    public class TaskItemMessageResolver : ITaskItemMessageResolver
    {
        private readonly TaskItemContext _itemContext;
        private readonly LinkCreator _linkCreator;
        private readonly Uri _baseUri;

        public TaskItemMessageResolver(TaskItemContext itemContext, LinkCreator linkCreator, Uri baseUri)
        {
            this._itemContext = itemContext;
            this._linkCreator = linkCreator;
            _baseUri = baseUri;
        }

        /// <summary>
        /// Resolves taskmessage to turn the taskID into a generated link that leads to another task with this Id
        /// </summary>
        /// <param name="taskItemMessage"></param>
        /// <returns></returns>
        public string? ResolveTaskMessage(TaskItemMessage taskItemMessage)
        {
            var resolvedMessage = taskItemMessage.Message;

            if (resolvedMessage == null) return null;

            if (taskItemMessage.ReferencedTaskIds != null)
            {
                foreach (var taskId in taskItemMessage.ReferencedTaskIds)
                {
                    var task = this.GetTaskById(taskId);
                    if (task?.Name != null)
                    {
                        var createdLink = _linkCreator.CreateLink<TaskItem>(_baseUri, task.Id);
                        resolvedMessage = resolvedMessage.Replace($"{{TASK_ID:{taskId}}}", $"{createdLink}");
                    }
                    else
                    {
                        resolvedMessage = resolvedMessage.Replace($"{{TASK_ID:{taskId}}}", "[TaskItem not found]");
                    }
                }
            }

            return resolvedMessage;
        }
        
        private TaskItem? GetTaskById(long id)
        {
            return this._itemContext.TaskItems.FirstOrDefault(x => x.Id == id);
        }
    }
}