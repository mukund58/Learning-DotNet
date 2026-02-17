using Microsoft.EntityFrameworkCore;

// namespace PostApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
            // var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    
            // optionsBuilder.UseNpgsql("connection_string");

    
            // return new AppDbContext(optionsBuilder.Options);
        }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}
