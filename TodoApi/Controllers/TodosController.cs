using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{   
    [ApiController]
    [Route("api/todo")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>GET /api/todo - list all items.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TodoResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var todos = await _todoService.GetAllTodosAsync(cancellationToken);
            return Ok(todos);
        }

        /// <summary>GET /api/todo/{id} - fetch a single item by id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TodoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var todo = await _todoService.GetTodoByIdAsync(id, cancellationToken);
            return Ok(todo);
        }

        /// <summary>POST /api/todo - create a new item.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(TodoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoResponse>> Create([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
        {
            var created = await _todoService.CreateTodoAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>PUT /api/todo/{id} - update an existing item.</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TodoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoResponse>> Update(int id, [FromBody] UpdateTodoRequest request, CancellationToken cancellationToken)
        {
            var updated = await _todoService.UpdateTodoAsync(id, request, cancellationToken);
            return Ok(updated);
        }

        /// <summary>DELETE /api/todo/{id} - delete a item.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _todoService.DeleteTodoAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
