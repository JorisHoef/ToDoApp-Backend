using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDoAppBackend.Models;
using ToDoAppBackend.Services;
using ToDoAppBackend.Logging;

namespace ToDoAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskItemContext _itemContext;
        private readonly ITaskItemMessageResolver _taskItemMessageResolver;
        private readonly TaskItemLogger _taskItemLogger;

        public TaskItemsController(TaskItemContext itemContext, ITaskItemMessageResolver taskItemMessageResolver, TaskItemLogger taskItemLogger)
        {
            _itemContext = itemContext;
            _taskItemMessageResolver = taskItemMessageResolver;
            _taskItemLogger = taskItemLogger;
        }
        
        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var taskItems = await _itemContext.TaskItems
                                              .Include(t => t.TaskItemMessage)
                                              .ToListAsync();

            foreach (var taskItem in taskItems)
            {
                ProcessTaskItem(taskItem);
            }
            return taskItems;
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(long id)
        {
            var taskItem = await _itemContext.TaskItems
                                 .Include(t => t.TaskItemMessage)
                                 .FirstOrDefaultAsync(t => t.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            
            ProcessTaskItem(taskItem);
            
            return taskItem;
        }

        // PUT: api/TaskItems/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> PutTask(long id, [FromBody] TaskItem taskItem)
        {
            if (taskItem == null)
            {
                return BadRequest("Task item cannot be null.");
            }

            if (id != taskItem.Id)
            {
                return BadRequest("ID mismatch.");
            }

            _taskItemLogger.LogInfo($"PUT request received with body: {JsonConvert.SerializeObject(taskItem)}");

            var existingTask = await _itemContext.TaskItems.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            try
            {
                // Update the existing task item
                existingTask.Name = taskItem.Name;
                existingTask.TaskItemMessage = taskItem.TaskItemMessage;
                existingTask.TaskDataState = taskItem.TaskDataState;
                existingTask.CreatedAt = taskItem.CreatedAt;
                existingTask.UpdatedAt = DateTime.UtcNow;
                existingTask.SubTasks = taskItem.SubTasks;

                await _itemContext.SaveChangesAsync();
                ProcessTaskItem(existingTask);
                return Ok(existingTask);
            }
            catch (DbUpdateException dbEx)
            {
                return _taskItemLogger.HandleDbUpdateException(dbEx);
            }
            catch (Exception ex)
            {
                return _taskItemLogger.HandleException(ex, existingTask);
            }
        }

        // POST: api/TaskItems
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItem taskItem)
        {
            if (taskItem == null)
            {
                return BadRequest("Task item cannot be null.");
            }

            _taskItemLogger.LogInfo($"POST request received with body: {JsonConvert.SerializeObject(taskItem)}");

            try
            {
                _itemContext.TaskItems.Add(taskItem);
                await _itemContext.SaveChangesAsync();
                ProcessTaskItem(taskItem);
                return CreatedAtAction(nameof(GetTask), new { id = taskItem.Id }, taskItem);
            }
            catch (DbUpdateException dbEx)
            {
                return _taskItemLogger.HandleDbUpdateException(dbEx);
            }
            catch (Exception ex)
            {
                var result = _taskItemLogger.HandleException(ex, taskItem);
                return result;
            }
        }
        
        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var taskItem = await _itemContext.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            
            _itemContext.TaskItems.Remove(taskItem);
            await _itemContext.SaveChangesAsync();
            
            return NoContent();
        }

        private bool TaskExists(long id)
        {
            return _itemContext.TaskItems.Any(e => e.Id == id);
        }

        private void ProcessTaskItem(TaskItem taskItem)
        {
            try
            {
                if (taskItem.TaskItemMessage != null)
                {
                    taskItem.TaskItemMessage.Message = _taskItemMessageResolver.ResolveTaskMessage(taskItem.TaskItemMessage);
                }

                if (taskItem.DeadlineAt >= DateTime.UtcNow)
                {
                    taskItem.TaskDataState = TaskDataState.STALE;
                }

                _taskItemLogger.LogTaskItem(taskItem);
            }
            catch (Exception ex)
            {
                _taskItemLogger.LogTaskItem(taskItem, ex);
            }
        }
    }
}
