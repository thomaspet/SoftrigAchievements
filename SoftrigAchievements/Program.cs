using Microsoft.OpenApi.Models;
using SoftrigAchievements;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using SoftrigAchievements.Controllers;
using Database;
using SoftrigAchievements.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("UniEcomony", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("uri:appframework"));
});
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultDb")));

builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IEconomyHttpService, EconomyHttpService>();


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
builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<Context>();
    dataContext.Database.Migrate();
}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(isOriginAllowed => true)
    .AllowCredentials());

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
app.MapPost("/webhooks/new-company", Webhooks.NewCompanyAddedWebhook).AllowAnonymous();
app.MapPost("/webhooks/listen/customerinvoice", Webhooks.HandleCustomerInvoiceWebhook).AllowAnonymous();

await app.RunAsync();
