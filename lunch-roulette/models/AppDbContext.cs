using lunch_roulette.models;
using Microsoft.EntityFrameworkCore;

namespace lunch_roulette;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Lunch> Lunches => Set<Lunch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Lunches)
            .WithMany(l => l.Persons)
            .UsingEntity(j => j.ToTable("PersonLunch"))
            .HasIndex(p => p.Name)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}