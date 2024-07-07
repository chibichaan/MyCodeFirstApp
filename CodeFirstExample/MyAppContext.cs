using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeFirstExample;

public class MyAppContext : DbContext, IDesignTimeDbContextFactory<MyAppContext>
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public MyAppContext(DbContextOptions<MyAppContext> options)
    {
        Database.EnsureCreated();
    }

    public MyAppContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=example;Username=postgres;Password=postgres");
    }
    
    public MyAppContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyAppContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=example;Username=postgres;Password=postgres");

        return new MyAppContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(p => p.Id);
        modelBuilder.Entity<Person>()
            .HasMany<Pet>(p => p.Pets)
            .WithOne(pet=> pet.Person);
        modelBuilder.Entity<Pet>().HasKey(p => p.Id);
    }
}
