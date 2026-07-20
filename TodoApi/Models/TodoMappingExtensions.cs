namespace TodoApi.Models
{
    public static class TodoMappingExtensions
    {
        public static TodoResponse ToResponse(this Todo todo) => new()
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CreatedAt = todo.CreatedAt
        };
    }
}
