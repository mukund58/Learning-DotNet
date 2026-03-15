namespace _24_API_Semantics.Models;
using Microsoft.EntityFrameworkCore;

public class TodoDbContext : DbContext {
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdempotencyRecord>()
            .HasIndex(x => x.Key)
            .IsUnique();
    }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }
}