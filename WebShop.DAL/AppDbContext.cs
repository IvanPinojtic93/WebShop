using Microsoft.EntityFrameworkCore;
using WebShop.DAL.Entities;

namespace WebShop.DAL;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; } = null!;

}