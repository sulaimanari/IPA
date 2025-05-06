using Microsoft.EntityFrameworkCore;
using scs_web_game.BusinessLogic;
using scs_web_game.DataAccessLayer;
using scs_web_game.Models;
using scs_web_game.Provider;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "FallbackConnectionString";


builder.Services.AddDbContext<WebGameContext>(options =>
    options.UseSqlServer(connectionString));


BuildServices(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Host.UseSerilog();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(p => p.WithOrigins("http://localhost:57138", "http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
Log.Information("running application...");

app.Run();
Log.CloseAndFlush();
return;

void BuildServices(WebApplicationBuilder webApplicationBuilder)
{
    builder.Services.AddDbContext<WebGameContext>();
    builder.Services.AddControllers();
    builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
    builder.Services.AddScoped<IEmployee, EmployeeBusinessLogic>();
    builder.Services.AddScoped<EmployeeBusinessLogic>();
    builder.Services.AddScoped<EmployeeDal>();
    builder.Services.AddScoped<IPlayer, PlayerBusinessLogic>();
    builder.Services.AddScoped<PlayerBusinessLogic>();
    builder.Services.AddScoped<PlayerDal>();
    builder.Services.AddScoped<IGame, GameBusinessLogic>();
    builder.Services.AddScoped<GameBusinessLogic>();
    builder.Services.AddScoped<GameDal>();
}