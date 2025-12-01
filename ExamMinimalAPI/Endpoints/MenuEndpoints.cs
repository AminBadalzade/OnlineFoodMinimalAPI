using OnlineFoodMinimalAPI.Models;

namespace OnlineFoodMinimalAPI.Endpoints
{
    public static class MenuEndpoints
    {
        public static void MapMenuEndpoints(this WebApplication app)
        {
            app.MapGet("/restaurants/menus", (
                int? restaurantId,
                string? name,
                string? description,
                double? minPrice,
                double? maxPrice,
                string? sortBy,
                bool? desc,
                IFoodRepository foodRepository
                ) =>
                {
                var menus = foodRepository.GetAllMenu(restaurantId, name, description, minPrice, maxPrice,
                      sortBy, desc);

                return Results.Ok(menus);
                      });

            app.MapGet("/restaurants/{id}/menu", (int id, IFoodRepository foodRepository) => {
                var restaurant = foodRepository.GetRestaurantById(id);
                if (restaurant is null)
                {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                  {
                        {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
                        });
                  }

                var menus = foodRepository.GetMenu(restaurant);

                return TypedResults.Ok(menus);

            });

            app.MapGet("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId, IFoodRepository foodRepository) => {
                var restaurant = foodRepository.GetRestaurantById(id);
                if (restaurant is null)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                   {
                       {"id", new[] {$"Restaurant with the id {id} doesn't exist"} }
                  });
                }

                var menu = foodRepository.GetMenuById(menuItemId, restaurant);

                return menu is not null
                ? TypedResults.Ok(menu)
                : Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                     {"menuItemId", new[] {$"Menu with the id {menuItemId} doesn't exist"} }
                    });
            });

            app.MapPost("/restaurants/{id}/menu", (int id, MenuItem menu, IFoodRepository foodRepository) => {
                // 1.Validation body
                if (menu is null)
                          {
                            return Results.ValidationProblem(new Dictionary<string, string[]>
                          {
                           {"menuItem", new[] {$"Menu item body is not valid"} }
                         });
                }

                // 2.Restaurant must exist
                var restaurant = foodRepository.GetRestaurantById(id);
                if (restaurant is null)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                     {
                         { "id", new[] { $"Restaurant with id {id} does not exist." } }
                     });
                }

                // 3.Ensure RestaurantID is correct
                menu.RestaurantId = id;

                foodRepository.AddMenu(menu);

                return TypedResults.Created($"/restaurants/{id:int}/menu/{menu.Id}", menu);
            }).WithParameterValidation();

            app.MapPut("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId, MenuItem menuItem, IFoodRepository foodRepository) =>
            {
                if (menuItem is null)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                         {
                    {"menuItem", new[] {$"Menu item body is not valid"} }
                          });
                }

                if (menuItem.Id != menuItemId)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                     {
                         { "menuId", new[] { "Menu item ID in body does not match route ID." } }
                    });
                }

                var restaurant = foodRepository.GetRestaurantById(id);
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

                var updated = foodRepository.UpdateMenuItem(menuItem);

                return updated
                   ? TypedResults.NoContent()
                   : Results.ValidationProblem(new Dictionary<string, string[]>
                   {
                         { "menuId", new[] { $"Menu item with id {menuItemId} does not exist." } }
                   });

                          });

            app.MapDelete("/restaurants/{id}/menu/{menuItemId:int}", (int id, int menuItemId, IFoodRepository foodRepository) => {
                var restaurant = foodRepository.GetRestaurantById(id);

                if (restaurant is null)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                      {
            { "Id", new[] { $"Restaurant with id {id} doesn't exist" } }
                       });
                }

                var menu = foodRepository.GetMenuById(menuItemId, restaurant);
                if (menu is null)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                      {
             { "menuId", new[] { $"Menu item with id {menuItemId} does not exist." } }
                        });
                }
                var IsDeleted = foodRepository.DeleteMenu(menu);


                return IsDeleted
                ? TypedResults.Ok(menu)
                : Results.ValidationProblem(new Dictionary<string, string[]>
                {
        {"menuItemId", new[] {$"Menu with the id {menuItemId} doesn't exist"} }
                });
            });

        }
    }
}
