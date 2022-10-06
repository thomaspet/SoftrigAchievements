using Microsoft.OpenApi.Models;
using SoftrigAchievements;
using DataCounter.Database;
using Microsoft.EntityFrameworkCore;
using Lib.AspNetCore.ServerSentEvents;
using DataPusher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<CounterDatabase>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddServerSentEvents();
builder.Services.AddHostedService<ServerEventsWorker>();
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

app.MapGet("/", () => "Helloworld!");

app.UseAuthentication();
app.UseAuthorization();

app.MapServerSentEvents("/sse-achievements");
await app.RunAsync();
