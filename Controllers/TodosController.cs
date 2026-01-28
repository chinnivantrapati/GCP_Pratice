using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCP_Pratice.Data;
using GCP_Pratice.Models;
using Microsoft.AspNetCore.Authorization;

namespace GCP_Pratice.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Add this attribute to protect the entire controller
    public class TodosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/todos
        /// <summary>
        /// Retrieves all todos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        // POST: api/todos
        /// <summary>
        /// Creates a new todo.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // GET: api/todos/1
        /// <summary>
        /// Retrieves a specific todo by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();
            return todo;
        }

        // PUT: api/todos/1
        /// <summary>
        /// Updates a specific todo.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todo"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/todos/1
        /// <summary>
        /// Deletes a specific todo.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TodoExists(int id)
        {
            return await _context.Todos.AnyAsync(e => e.Id == id);
        }
    }
}