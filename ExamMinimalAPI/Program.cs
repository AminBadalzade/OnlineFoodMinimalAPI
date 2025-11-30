using OnlineFoodMinimalAPI.Models;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();



var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

app.MapGet("/", () => "It's Online Food MinimalAPI");

app.MapGet("/restaurants", (
    string? name,
    string? category,
    double? minRating,
    double? maxRating,
    string? sortBy,
    bool? desc) => {

    var restaurants = FoodRepository.GetRestaurants(name,category,minRating,maxRating,sortBy,desc);

    return Results.Ok(restaurants);
});


app.MapGet("/restaurants/{id:int}", (int id) => {
    var restaurant = FoodRepository.GetRestaurantById(id);

    return restaurant is not null
    ? TypedResults.Ok(restaurant)
    : Results.ValidationProblem(new Dictionary<string, string[]>
    {
         {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
    });
});

app.MapPost("/restaurants", (Restaurant restaurant) =>
{
    if (restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
           {
             { "restaurant", new[] { "Restaurant is not provided." } }
           });
    }

     FoodRepository.AddRestaurant(restaurant);

    return TypedResults.Created($"/restaurants/{restaurant.Id}", restaurant);

}).WithParameterValidation();

app.MapPut("/restaurants/{id:int}", (int id, Restaurant restaurant) =>
{
    if(restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {"restaurant", new[] {$"Restaurant body is missing or invali" } }
        });
    }

    if (id != restaurant.Id)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {"id", new[] {$"Restaurant id is not valid"} }
        });
    }

    var updated = FoodRepository.UpdateRestaurant(restaurant);

    return updated
    ? TypedResults.NoContent() : Results.ValidationProblem(new Dictionary<string, string[]>
    {
        {"id", new[] {$"Restaurant with id {id} is not valid"} }
    });
});

app.MapDelete("/restaurants/{id:int}", (int id) => {
    var restaurant = FoodRepository.GetRestaurantById(id);

    if(restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            {"id", new[] {$"Restaurant id is not valid"} }
        });
    }

    var IsDeleted = FoodRepository.DeleteRestaurant(restaurant);

    return IsDeleted
    ? TypedResults.Ok(restaurant)
    : Results.ValidationProblem(new Dictionary<string, string[]>
    {
         {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
    });
});

app.MapGet("/restaurants/menus", (
    int? restaurantId,
    string? name,
    string? description,
    double? minPrice,
    double? maxPrice,
    string? sortBy,
    bool? desc
    ) =>
{
    var menus = FoodRepository.GetAllMenu(restaurantId,name,description,minPrice,maxPrice,
          sortBy,desc);

    return Results.Ok(menus);
});

app.MapGet("/restaurants/{id}/menu", (int id) => {
    var restaurant = FoodRepository.GetRestaurantById(id);
    if (restaurant is  null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
         {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
        });
    }

    var menus = FoodRepository.GetMenu(restaurant);

    return TypedResults.Ok(menus);

});

app.MapGet("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId) => {
    var restaurant = FoodRepository.GetRestaurantById(id);
    if (restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
         {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
        });
    }

    var menu = FoodRepository.GetMenuById(menuItemId, restaurant);

    return menu is not null
    ? TypedResults.Ok(menu)
    : Results.ValidationProblem(new Dictionary<string, string[]>
    {
        {"menuItemId", new[] {$"Menu with the id {menuItemId} doesn't exist"} }
    });
});

app.MapPost("/restaurants/{id}/menu", (int id, MenuItem menu) => {
    // 1.Validation body
    if(menu is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
            {
        {"menuItem", new[] {$"Menu item body is not valid"} }
             });
    }

    // 2.Restaurant must exist
    var restaurant = FoodRepository.GetRestaurantById(id);
    if (restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { $"Restaurant with id {id} does not exist." } }
        });
    }

    // 3.Ensure RestaurantID is correct
    menu.RestaurantId = id;

    FoodRepository.AddMenu(menu);

    return TypedResults.Created($"/restaurants/{id:int}/menu/{menu.Id}", menu);
}).WithParameterValidation();

app.MapPut("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId, MenuItem menuItem) =>
{
    if (menuItem is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
            {
        {"menuItem", new[] {$"Menu item body is not valid"} }
             });
    }

    if(menuItem.Id != menuItemId)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "menuId", new[] { "Menu item ID in body does not match route ID." } }
        });
    }

    var restaurant = FoodRepository.GetRestaurantById(id);
    if (restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { $"Restaurant with id {id} does not exist." } }
        });
    }

    if (menuItem.RestaurantId != id)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "restaurantId", new[] { "Menu item does not belong to this restaurant." } }
        });
    }

    var updated = FoodRepository.UpdateMenuItem(menuItem);

    return updated
       ? TypedResults.NoContent()
       : Results.ValidationProblem(new Dictionary<string, string[]>
       {
            { "menuId", new[] { $"Menu item with id {menuItemId} does not exist." } }
       });

});

app.MapDelete("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId) => {
    var restaurant = FoodRepository.GetRestaurantById(id);

    if (restaurant is null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "Id", new[] { $"Restaurant with id {id} doesn't exist" } }
        });
    }

    var menu = FoodRepository.GetMenuById(menuItemId, restaurant);
    if(menu is null)
    {
       return Results.ValidationProblem(new Dictionary<string, string[]>
       {
            { "menuId", new[] { $"Menu item with id {menuItemId} does not exist." } }
       });
    }
    var IsDeleted = FoodRepository.DeleteMenu(menu);


    return IsDeleted
    ? TypedResults.Ok(menu)
    : Results.ValidationProblem(new Dictionary<string, string[]>
    {
        {"menuItemId", new[] {$"Menu with the id {menuItemId} doesn't exist"} }
    });
});



app.Run();
