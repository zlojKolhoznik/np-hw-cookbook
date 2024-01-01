using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Data;

public class CookBookContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; } = null!;
    
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    public CookBookContext(DbContextOptions<CookBookContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ingredient>().HasIndex(i => i.Name).IsUnique();
    }
}