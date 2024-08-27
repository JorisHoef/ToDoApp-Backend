using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppBackend.Models;
using ToDoAppBackend.Services;

namespace ToDoAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskItemContext _itemContext;
        private readonly ITaskItemMessageResolver _taskItemMessageResolver;
        
        public TaskItemsController(TaskItemContext itemContext, ITaskItemMessageResolver taskItemMessageResolver)
        {
            this._itemContext = itemContext;
            this._taskItemMessageResolver = taskItemMessageResolver;
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
                if (taskItem.TaskItemMessage != null)
                {
                    taskItem.TaskItemMessage.Message = _taskItemMessageResolver.ResolveTaskMessage(taskItem.TaskItemMessage);
                }
            }
            return taskItems;
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(long id)
        {
            var taskItem = await this._itemContext.TaskItems
                                 .Include(t => t.TaskItemMessage)
                                 .FirstOrDefaultAsync(t => t.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            
            if (taskItem.TaskItemMessage != null)
            {
                var resolvedTaskMessage = this._taskItemMessageResolver.ResolveTaskMessage(taskItem.TaskItemMessage);
                taskItem.TaskItemMessage.Message = resolvedTaskMessage;
            }
            
            return taskItem;
        }
        
        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(long id, [FromBody] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }
            
            var existingTask = await this._itemContext.TaskItems.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }
            
            // Update the existing taskItem item
            existingTask.Name = taskItem.Name;
            existingTask.TaskItemMessage = taskItem.TaskItemMessage;
            existingTask.TaskDataState = taskItem.TaskDataState;
            existingTask.CreatedAt = taskItem.CreatedAt;
            existingTask.UpdatedAt = DateTime.Now;
            existingTask.SubTasks = taskItem.SubTasks;
            
            try
            {
                await this._itemContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!this.TaskExists(id))
            {
                return NotFound();
            }
    
            // Return the updated task item with a 200 OK status
            return Ok(existingTask);
        }
        
        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItem taskItem)
        {
            this._itemContext.TaskItems.Add(taskItem);
            await this._itemContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(this.GetTask), new { id = taskItem.Id }, taskItem);
        }
        
        // DELETE: api/TaskItems/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var taskItem = await this._itemContext.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            
            this._itemContext.TaskItems.Remove(taskItem);
            await this._itemContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        private bool TaskExists(long id)
        {
            return this._itemContext.TaskItems.Any(e => e.Id == id);
        }
    }
}