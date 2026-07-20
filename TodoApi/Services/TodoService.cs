using TodoApi.Models;
using TodoApi.Exceptions;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ITodoRepository repository, ILogger<TodoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TodoResponse> CreateTodoAsync(CreateTodoRequest request, CancellationToken cancellationToken = default)
        {
            var todo = new Todo
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                IsCompleted = request.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(todo, cancellationToken);
            _logger.LogInformation("Created todo {TodoId}", created.Id);
            return created.ToResponse();
        }

        public async Task<List<TodoResponse>> GetAllTodosAsync(CancellationToken cancellationToken = default)
        {
            var todos = await _repository.GetAllAsync(cancellationToken);
            return todos.Select(t => t.ToResponse()).ToList();
        }

        public async Task<TodoResponse> GetTodoByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var todo = await _repository.GetByIdAsync(id, cancellationToken);
            if (todo is null)
            {
                throw new TodoNotFoundException(id);
            }

            return todo.ToResponse();
        }

        public async Task<TodoResponse> UpdateTodoAsync(int id, UpdateTodoRequest request, CancellationToken cancellationToken = default)
        {
            var todo = new Todo
            {
                Id = id,
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                IsCompleted = request.IsCompleted
            };

            var updated = await _repository.UpdateAsync(todo, cancellationToken);
            if (!updated)
            {
                throw new TodoNotFoundException(id);
            }

            _logger.LogInformation("Updated todo {TodoId}", id);

            // Re-fetch so the response reflects the persisted state (e.g. CreatedAt).
            var persisted = await _repository.GetByIdAsync(id, cancellationToken);
            return persisted!.ToResponse();
        }

        public async Task DeleteTodoAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _repository.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                throw new TodoNotFoundException(id);
            }

            _logger.LogInformation("Deleted todo {TodoId}", id);
        }
    }
}
