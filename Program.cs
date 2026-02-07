using Microsoft.EntityFrameworkCore;
using Serilog;
using StockFlow.API.Data;
using StockFlow.API.Middleware;
using StockFlow.API.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/stockflow-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting StockFlow API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "StockFlow API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
