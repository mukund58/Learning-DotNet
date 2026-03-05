using Microsoft.EntityFrameworkCore;
namespace _20_curd_mastery.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();  
    public DbSet<Todo> Todos => Set<Todo>();
}
