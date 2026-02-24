using Microsoft.EntityFrameworkCore;
using 17-dtos-vs-entities.DTOs;

namespace SimpleApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}

