using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppBackend.Models;

namespace ToDoAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTaskItems()
        {
            return await _context.TaskItems
                                 .Select(x => ItemToDTO(x))
                                 .ToListAsync();
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDTO>> GetTaskItem(long id)
        {
            var taskItem = await _context.TaskItems
                                         .Include(t => t.TaskMessage)
                                         .FirstOrDefaultAsync(t => t.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }
            
            var resolvedTaskMessage = this.ResolvedTaskMessage(taskItem.TaskMessage);
            taskItem.TaskMessage.Message = resolvedTaskMessage;
            return ItemToDTO(taskItem);
        }
        
        /// <summary>
        /// Resolves taskmessage to turn the taskID into a generated link that leads to another task with this Id
        /// </summary>
        /// <param name="taskMessage"></param>
        /// <returns></returns>
        private string? ResolvedTaskMessage(TaskMessage taskMessage)
        {
            var resolvedMessage = taskMessage.Message;

            if (resolvedMessage == null) return null;
            
            foreach (var taskId in taskMessage.ReferencedTaskIds)
            {
                var task = GetTaskById(taskId);
                resolvedMessage = resolvedMessage.Replace($"{{{taskId}}}", $"<a href='/tasks/{taskId}'>{task.Name}</a>"); //Can apparently create links from DB, thanks chatGPT
            }

            return resolvedMessage;
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(long id, TaskItemDTO taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Name = taskDto.Name;
            taskItem.TaskMessage = taskDto.TaskMessage;
            taskItem.TaskState = taskDto.TaskState;
            taskItem.CreatedAt = taskDto.CreatedAt;
            taskItem.UpdatedAt = DateTime.Now;
            taskItem.SubTasks = taskDto.SubTasks;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!this.TaskItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItemDTO>> PostTaskItem(TaskItemDTO taskDto)
        {
            var todoItem = new TaskItem
            {
                    TaskState = taskDto.TaskState,
                    Name = taskDto.Name
            };

            _context.TaskItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                                   nameof(this.GetTaskItem),
                                   new { id = todoItem.Id },
                                   ItemToDTO(todoItem));
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(long id)
        {
            var todoItem = await _context.TaskItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
        
        private TaskItem GetTaskById(long id)
        {
            return this._context.TaskItems.First(x => x.Id == id);
        }

        /// <summary>
        /// Is similar to what some devs might call a "ResourceModel", where we only transfer and use what we want from a bigger dataModel
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        /// <remarks>Not needed for this app but keep here as reminder that this goes in the controller</remarks>
        private static TaskItemDTO ItemToDTO(TaskItem taskItem)
        {
            return new TaskItemDTO
            {
                    Id = taskItem.Id, 
                    Name = taskItem.Name, 
                    TaskMessage = taskItem.TaskMessage,
                    TaskState = taskItem.TaskState,
                    CreatedAt = taskItem.CreatedAt,
                    UpdatedAt = taskItem.UpdatedAt,
                    SubTasks = taskItem.SubTasks,
            };
        }
    }
}