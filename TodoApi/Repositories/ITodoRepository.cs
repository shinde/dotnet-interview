using TodoApi.Models;

namespace TodoApi.Repositories
{
    /// <summary>
    /// Persistence-only abstraction over Todo storage. No business rules live here -
    /// that belongs in ITodoService. Kept separate so the data access technology
    /// (currently EF Core/SQLite) can be swapped or mocked without touching business logic.
    /// </summary>
    public interface ITodoRepository
    {
        Task<Todo> AddAsync(Todo todo, CancellationToken cancellationToken = default);

        Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Returns true if a row with this id existed and was updated.</summary>
        Task<bool> UpdateAsync(Todo todo, CancellationToken cancellationToken = default);

        /// <summary>Returns true if a row with this id existed and was deleted.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
