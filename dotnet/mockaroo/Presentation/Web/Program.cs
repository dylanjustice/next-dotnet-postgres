using Data.Mockaroo.Startup;
using Mockaroo.Business.Conductors.Extensions;

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

app.Run();
