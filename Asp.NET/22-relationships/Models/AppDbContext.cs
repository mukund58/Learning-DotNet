namespace _22_relationships.Models;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeAddress)
            .WithOne(e => e.Employee)
            .HasForeignKey<EmployeeAddress>(a => a.EmployeeId);
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
}