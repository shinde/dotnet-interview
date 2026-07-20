using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("Todos");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(t => t.Description)
                    .HasMaxLength(2000);
                entity.Property(t => t.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
