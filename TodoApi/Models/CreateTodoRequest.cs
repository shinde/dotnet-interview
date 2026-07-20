using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class CreateTodoRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
