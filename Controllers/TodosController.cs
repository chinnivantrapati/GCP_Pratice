using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCP_Pratice.Data;
using GCP_Pratice.Models;

namespace GCP_Pratice.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }
    }
}