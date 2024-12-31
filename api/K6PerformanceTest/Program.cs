using System.Collections.Generic;
using K6PerformanceTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AppDb"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 10).Select(index =>
        new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/dbWeatherforecast", (int page, int count, [FromServices] AppDbContext dbContext) =>
{
    page = page == 0 ? 1 : page;
    count = count == 0 ? 10 : count;
    var forecast = dbContext.WeatherForecasts
    .OrderBy(x => x.Date)
    .Skip((page - 1) * count)
    .Take(count)
    .ToArrayAsync();

    return forecast;
})
.WithName("GetWeatherForecastFromDb")
.WithOpenApi();

app.MapGet("/dbWeatherforecastAsNoTracking", (int page, int count, [FromServices] AppDbContext dbContext) =>
{
    page = page == 0 ? 1 : page;
    count = count == 0 ? 10 : count;
    var forecast = dbContext.WeatherForecasts
    .AsNoTracking()
    .OrderBy(x => x.Date)
    .Skip((page - 1) * count)
    .Take(count)
    .ToArrayAsync();

    return forecast;
})
.WithName("GetWeatherForecastFromDbAsNoTracking")
.WithOpenApi();

app.MapGet("/dbWeatherforecastEnhanced", async (int page, int count, [FromServices] AppDbContext dbContext) =>
{
    page = page == 0 ? 1 : page;
    count = count == 0 ? 10 : count;
    FormattableString query = $@" SELECT *, ROW_NUMBER() OVER (ORDER BY [Id]) AS [RowNumber] FROM [WeatherForecasts] ";

    var forecasts = dbContext.Database
    .SqlQuery<WeatherForecastWithRowNumber>(query)
    .Where(e => e.RowNumber > (page - 1) * count && e.RowNumber <= page * count)
    .ToList();
    
    return forecasts;
})
.WithName("GetWeatherForecastFromDbEnhanced")
.WithOpenApi();


app.Run();
