namespace TodoApi.Exceptions
{
    /// <summary>
    /// Thrown when a requested TODO item does not exist. Caught by    
    /// </summary>
    public class TodoNotFoundException : Exception
    {
        public TodoNotFoundException(int id) : base($"Todo with id {id} was not found.")
        {
            Id = id;
        }

        public int Id { get; }
    }
}
