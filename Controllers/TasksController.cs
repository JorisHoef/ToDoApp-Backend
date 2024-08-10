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

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                                 .Select(x => ItemToDTO(x))
                                 .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TaskItemDTO taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = taskDto.Name;
            todoItem.TaskState = taskDto.TaskState;
            todoItem.CreatedAt = taskDto.CreatedAt;
            todoItem.UpdatedAt = DateTime.Now;
            todoItem.SubTasks = taskDto.SubTasks;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItemDTO>> PostTodoItem(TaskItemDTO taskDto)
        {
            var todoItem = new TaskItem
            {
                    TaskState = taskDto.TaskState,
                    Name = taskDto.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                                   nameof(GetTodoItem),
                                   new { id = todoItem.Id },
                                   ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
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
                    TaskState = taskItem.TaskState,
                    CreatedAt = taskItem.CreatedAt,
                    UpdatedAt = taskItem.UpdatedAt,
                    SubTasks = taskItem.SubTasks,
            };
        }
    }
}