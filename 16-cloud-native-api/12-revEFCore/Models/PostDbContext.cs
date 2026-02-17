using Microsoft.EntityFrameworkCore;
using  revEFCore.Models;

public class PostDbContext : DbContext
{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlite("Data Source = task.db");
        }    

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}
