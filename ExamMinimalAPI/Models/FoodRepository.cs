namespace OnlineFoodMinimalAPI.Models
{
    public static class FoodRepository
    {
        /* Restaurants data */

        private static List<Restaurant> _restaurants = new List<Restaurant>()
        {
            new Restaurant(1, "Pizza Palace", "Italian", "123 Main St, CityA", 4, "10:00-22:00"),
            new Restaurant(2, "Sushi World", "Japanese", "456 Ocean Ave, CityB", 5, "11:00-21:00"),
            new Restaurant(3, "Burger Hub", "Fast Food", "789 Elm St, CityC", 3, "09:00-23:00"),
            new Restaurant(4, "Curry Corner", "Indian", "321 Spice Rd, CityD", 4, "10:30-22:30"),
            new Restaurant(5, "Vegan Delight", "Vegan", "654 Green St, CityE", 5, "08:00-20:00"),
            new Restaurant(6, "Taco Town", "Mexican", "987 Fiesta Ave, CityF", 4, "10:00-00:00")
        };

        /* Methods of route starting with Restaurant */

        public static List<Restaurant> GetRestaurants(
            string? name, 
            string? category,
            double? minRating,
            double? maxRating,
            string? sortBy,
            bool? desc
            )
        {
            IEnumerable<Restaurant> query = _restaurants;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(r => r.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (minRating.HasValue)
                query = query.Where(r => r.Rating >= minRating.Value);

            if (maxRating.HasValue)
                query = query.Where(r => r.Rating <= maxRating.Value);

            query = sortBy?.ToLower() switch
            {
                "name" => (desc ?? false) ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                "rating" => (desc ?? false) ? query.OrderByDescending(r => r.Rating) : query.OrderBy(r => r.Rating),
                "category" => (desc ?? false) ? query.OrderByDescending(r => r.Category) : query.OrderBy(r => r.Category),
                _ => query
            };

            return query.ToList();
        }

        public static Restaurant GetRestaurantById(int id)
        {
            return _restaurants.FirstOrDefault(x => x.Id == id);

        }

        public static void AddRestaurant(Restaurant restaurant)
        {
            if(restaurant is not null)
            {
                int maxId = _restaurants.Max(x => x.Id);
                restaurant.Id = maxId + 1;
                _restaurants.Add(restaurant);
            }
        }

        public static bool UpdateRestaurant(Restaurant restaurant)
        {
            var res = _restaurants.FirstOrDefault(x => x.Id == restaurant.Id);
            if(res is not null)
            {
                res.Name = restaurant.Name;
                res.Address = restaurant.Address;
                res.Category = restaurant.Category;
                res.Rating = restaurant.Rating;
                res.OpenHours = restaurant.OpenHours;

                return true;
            }

            return false;
        }

        public static bool DeleteRestaurant(Restaurant restaurant)
        {
           if(restaurant is not null)
            {
                _restaurants.Remove(restaurant);
                return true;
            }

            return false;

        }

        /* Restaurants menu's data */


        private static List<MenuItem> _menuItems = new List<MenuItem>
        {
            new MenuItem { Id = 1, RestaurantId = 1, Name = "Margherita Pizza", Description = "Classic cheese & tomato", Price = 8.99d, Tags = "Italian,Pizza" },
            new MenuItem { Id = 2, RestaurantId = 1, Name = "Pepperoni Pizza", Description = "Pepperoni, cheese, tomato", Price = 10.99d, Tags = "Italian,Pizza" },

            new MenuItem { Id = 3, RestaurantId = 2, Name = "Salmon Sushi", Description = "Fresh salmon sushi rolls", Price = 12.50d, Tags = "Japanese,Sushi" },
            new MenuItem { Id = 4, RestaurantId = 2, Name = "Tuna Nigiri", Description = "Tuna on rice, classic nigiri", Price = 11.00d, Tags = "Japanese,Sushi" },

            new MenuItem { Id = 5, RestaurantId = 3, Name = "Cheeseburger", Description = "Beef patty with cheddar", Price = 9.50d, Tags = "FastFood,Burger" },
            new MenuItem { Id = 6, RestaurantId = 3, Name = "Fries", Description = "Crispy golden fries", Price = 3.00d, Tags = "FastFood,Side" },

            new MenuItem { Id = 7, RestaurantId = 4, Name = "Chicken Curry", Description = "Spicy chicken curry", Price = 11.50d, Tags = "Indian,Curry" },
            new MenuItem { Id = 8, RestaurantId = 4, Name = "Vegetable Samosa", Description = "Fried pastry with veggies", Price = 4.50d, Tags = "Indian,Snack" },

            new MenuItem { Id = 9, RestaurantId = 5, Name = "Vegan Wrap", Description = "Healthy wrap with veggies", Price = 7.99d, Tags = "Vegan,Wrap" },
            new MenuItem { Id = 10, RestaurantId = 5, Name = "Smoothie Bowl", Description = "Fresh fruit & granola", Price = 6.50d, Tags = "Vegan,Breakfast" },

            new MenuItem { Id = 11, RestaurantId = 6, Name = "Beef Taco", Description = "Soft tortilla, beef, salsa", Price = 5.99d, Tags = "Mexican,Taco" },
            new MenuItem { Id = 12, RestaurantId = 6, Name = "Chicken Quesadilla", Description = "Cheese & chicken tortilla", Price = 6.99d, Tags = "Mexican,Cheese" }
        };

        /* Methods of route with menu */
        public static List<MenuItem> GetMenu(Restaurant restaurant)
        {
            return _menuItems.Where(x => x.RestaurantId == restaurant.Id).ToList();
        }

        public static List<MenuItem> GetAllMenu(int? restaurantId,
            string? name,
            string? description,
            double? minPrice,
            double? maxPrice, 
            string? sortBy,
            bool? desc)
            { 
            IEnumerable<MenuItem> query = _menuItems;

            if (name is not null)
                query = query.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if(description is not null)
                query = query.Where(x => x.Name.Contains(description, StringComparison.OrdinalIgnoreCase));
            if (minPrice.HasValue)
                query = query.Where(r => r.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice.Value);

            query = sortBy?.ToLower() switch
            {
                "name" => (desc ?? false) ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "price" => (desc ?? false) ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                _ => query
            };

            return query.ToList();

        }

        public static MenuItem GetMenuById(int id, Restaurant restaurant)
        {
            var menus = GetMenu(restaurant);
            return menus.FirstOrDefault(x => x.Id == id);
        }

        public static void AddMenu(MenuItem menu)
        {
            int menuId = _menuItems.Max(x => x.Id);
            menu.Id = menuId + 1;
            _menuItems.Add(menu);
        }

        public static bool UpdateMenuItem(MenuItem menuItem)
        {
            var menu = _menuItems.FirstOrDefault(x => x.Id == menuItem.Id);
            if (menu is not null)
            {
                menu.Name = menuItem.Name;
                menu.RestaurantId = menuItem.RestaurantId;
                menu.Description = menuItem.Description;
                menu.Price = menuItem.Price;
                menu.Tags = menuItem.Tags;

                return true;
            }

            return false;
        }

        public static bool DeleteMenu(MenuItem menuItem)
        {
            if(menuItem is not null)
            {
                _menuItems.Remove(menuItem);
                return true;
            }
            return false;
        }
    }
}
