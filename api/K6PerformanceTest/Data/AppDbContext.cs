using Microsoft.EntityFrameworkCore;

namespace K6PerformanceTest.Data;

public class AppDbContext : DbContext
{
    string[] summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<WeatherForecast>()
            .HasData(Enumerable.Range(1, 1000).Select(index =>
        new WeatherForecast
        {
            Id = index,
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }));
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}
