using Microsoft.OpenApi.Models;
using SoftrigAchievements;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using SoftrigAchievements.Controllers;
using Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Softrig Achievements",
        Description = "Aggregates and counts your data to provide achievements!",
        Version = "v1"
    });
});
builder.Services.AddUniAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read", p => p.RequireAuthenticatedUser());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Softrig Achievements V1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/new-achievements", NewAchievements.GetNewAchievements).RequireAuthorization();
app.MapGet("/api/all-achievements", AllAchievements.GetAllAchievements).RequireAuthorization();

await app.RunAsync();
