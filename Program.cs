using Microsoft.EntityFrameworkCore;
using GCP_Pratice.Data; 
using System;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --- START: DATABASE CONFIGURATION ---
var connectionStringWithoutPassword = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (string.IsNullOrEmpty(connectionStringWithoutPassword))
{
    throw new InvalidOperationException("DefaultConnection connection string is not configured in appsettings or environment variables.");
}

if (string.IsNullOrEmpty(dbPassword) && !builder.Environment.IsDevelopment())
{
    throw new InvalidOperationException("DB_PASSWORD environment variable is not set. This is required for production deployments.");
}

var finalConnectionString = $"{connectionStringWithoutPassword};Pwd={dbPassword}";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(finalConnectionString, ServerVersion.AutoDetect(finalConnectionString))
);
// --- END: DATABASE CONFIGURATION ---

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
