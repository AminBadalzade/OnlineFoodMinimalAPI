using OnlineFoodMinimalAPI.Models;
using System.Globalization;
using System.Text.Json.Serialization;
using OnlineFoodMinimalAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IFoodRepository, FoodRepository>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.MapGet("/", () => "It's Online Food MinimalAPI");

app.MapRestaurantEndpoints();

app.MapMenuEndpoints();


app.Run();
