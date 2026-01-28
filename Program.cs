using Microsoft.EntityFrameworkCore;
using GCP_Pratice.Data; 
using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// --- START: JWT AUTHENTICATION CONFIGURATION ---
var jwtSigningKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtSigningKey))
{
    throw new InvalidOperationException("JWT_SIGNING_KEY environment variable or Jwt:Key configuration is not set.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
        };
    });

builder.Services.AddAuthorization();
// --- END: JWT AUTHENTICATION CONFIGURATION ---


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

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
