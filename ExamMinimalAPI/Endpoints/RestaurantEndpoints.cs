using OnlineFoodMinimalAPI.Models;

namespace OnlineFoodMinimalAPI.Endpoints
{
    public static class RestaurantEndpoints
    {
        public static void MapRestaurantEndpoints(this WebApplication app)
        {
            app.MapGet("/restaurants", (
    string? name,
    string? category,
    double? minRating,
    double? maxRating,
    string? sortBy,
    bool? desc) => {

        var restaurants = FoodRepository.GetRestaurants(name, category, minRating, maxRating, sortBy, desc);

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
                if (restaurant is null)
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

                if (restaurant is null)
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
        }
    }
}
