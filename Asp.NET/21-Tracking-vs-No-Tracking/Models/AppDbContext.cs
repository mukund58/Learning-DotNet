namespace _21_Tracking_vs_No_Tracking.Models;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set;}
}