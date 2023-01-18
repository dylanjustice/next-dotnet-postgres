using Data.Mockaroo.Startup;
using Mockaroo.Business.Conductors.Extensions;
using Mockaroo.Presentation.Web.Constants;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configurationBuilder = builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();

var config = configurationBuilder.Build();
builder.Services.AddConductors(config);
builder.Services.AddMockaroo(config);
builder.Services.AddAutoMapper(typeof(Mockaroo.Infrastructure.Data.Mockaroo.Maps.MappingProfile));
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
        outputTemplate: LoggingConstants.OUTPUT_TEMPLATE,
        theme: SystemConsoleTheme.Colored)
    .MinimumLevel.Information()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Application", LoggingConstants.APP_NAME));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
