using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<Todo> AddAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync(cancellationToken);
            return todo;
        }

        public async Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Todos
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Todo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Todos.FirstOrDefaultAsync(t => t.Id == todo.Id, cancellationToken);
            if (existing is null)
            {
                return false;
            }

            existing.Title = todo.Title;
            existing.Description = todo.Description;
            existing.IsCompleted = todo.IsCompleted;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (existing is null)
            {
                return false;
            }

            _context.Todos.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
