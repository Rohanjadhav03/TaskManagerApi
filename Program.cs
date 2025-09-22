using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Repositories;
using TaskManagerApi.Services;
using TaskManagerApi.Profiles;

var builder = WebApplication.CreateBuilder(args);

// === Configuration & Connection String ===
// Put your real password in appsettings.json (or use env var in production)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "server=localhost;port=3306;database=TaskManagerDB;user=taskuser;password=root";

// === Services ===
builder.Services.AddControllers();

// EF Core + Pomelo MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Repositories & Services (scoped)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// optional: keep any helper extension you used previously (AddOpenApi)
try
{
    // If you had a custom AddOpenApi extension earlier, keep it safe:
    builder.Services.AddOpenApi();
}
catch
{
    // no-op if method not available
}

var app = builder.Build();

// === Apply migrations & seed DB at startup (development-friendly) ===
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        // apply migrations (create tables)
        db.Database.Migrate();
        // seed default tasks (only if none)
        DbSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        // log to console - replace with logger if you add one
        Console.WriteLine("Error while migrating/seeding database: " + ex);
    }
}

// === Middleware / Pipeline ===
if (app.Environment.IsDevelopment())
{
    // use swagger only in development by default
    app.UseSwagger();
    app.UseSwaggerUI();
    try
    {
        // If you used MapOpenApi in dev previously
        app.MapOpenApi();
    }
    catch
    {
        // ignore if not present
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// --- keep the weather sample endpoint for quick testing ---
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}