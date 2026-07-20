using TodoApi.Models;

namespace TodoApi.Services
{   
    public interface ITodoService
    {
        Task<TodoResponse> CreateTodoAsync(CreateTodoRequest request, CancellationToken cancellationToken = default);
        Task<List<TodoResponse>> GetAllTodosAsync(CancellationToken cancellationToken = default);
        Task<TodoResponse> GetTodoByIdAsync(int id, CancellationToken cancellationToken = default);        
        Task<TodoResponse> UpdateTodoAsync(int id, UpdateTodoRequest request, CancellationToken cancellationToken = default);
        Task DeleteTodoAsync(int id, CancellationToken cancellationToken = default);
    }
}
