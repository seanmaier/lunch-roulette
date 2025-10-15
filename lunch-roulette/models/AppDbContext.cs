using lunch_roulette.models;
using Microsoft.EntityFrameworkCore;

namespace lunch_roulette;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Lunch> Lunches => Set<Lunch>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMembers> GroupMembers => Set<GroupMembers>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasIndex(p => p.Name)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}