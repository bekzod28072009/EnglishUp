using Auth.Api.Configurations;
using Auth.Api.Extensions;
using Auth.DataAccess.AppDbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database context  
builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.  
builder.Services.AddServices(); // Custom extension method to add services  
// Fix for CS1503: Use a lambda to configure AutoMapper  
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapConfiguration).Assembly));




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "EnglishUp API",
        Version = "v1",
        Description = "An API for the English learning platform"
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi  
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
