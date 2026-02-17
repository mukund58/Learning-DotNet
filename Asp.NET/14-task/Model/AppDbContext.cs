using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    // Required constructor to pass configuration options
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // Define your tables here
    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Viraj" },
            new User { Id = 2, Name = "mukund" }
        );
    
        // Seed Tasks (ensure the UserId matches an existing User)
        modelBuilder.Entity<Task>().HasData(
            new Task { TaskId = 1, Name = "Learn ASP.NET Core"},
            new Task { TaskId = 2, Name = "Setup SQLite" }
        );
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         @"Server=(localdb)\mssqllocaldb;Database=Test;ConnectRetryCount=0");
    // }
}
