using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public ActionResult<TaskItem> HandleDbUpdateException(DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error");
            return new BadRequestObjectResult($"Database update error: {ex.Message}");
        }

        public ActionResult<TaskItem> HandleException(Exception ex, TaskItem? taskItem = null)
        {
            if (taskItem != null)
            {
                LogTaskItem(taskItem, ex);
            }
            else
            {
                _logger.LogError(ex, "An error occurred");
            }
            
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}