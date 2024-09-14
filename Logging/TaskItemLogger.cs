using ToDoAppBackend.Models;

namespace ToDoAppBackend.Logging
{
    public class TaskItemLogger
    {
        private readonly ILogger<TaskItemLogger> _logger;

        public TaskItemLogger(ILogger<TaskItemLogger> logger)
        {
            _logger = logger;
        }

        public void LogTaskItem(TaskItem taskItem, Exception? ex = null)
        {
            var taskInfo = $"Task ID: {taskItem.Id}, Name: {taskItem.Name}, State: {taskItem.TaskDataState}";

            if (ex != null)
            {
                _logger.LogError(ex, $"Error processing task. {taskInfo}");
            }
            else
            {
                _logger.LogInformation($"Successfully processed task. {taskInfo}");
            }
        }
    }
}