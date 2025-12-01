
namespace OnlineFoodMinimalAPI.Models
{
    public interface IFoodRepository
    {
        void AddMenu(MenuItem menu);
        void AddRestaurant(Restaurant restaurant);
        bool DeleteMenu(MenuItem menuItem);
        bool DeleteRestaurant(Restaurant restaurant);
        List<MenuItem> GetAllMenu(int? restaurantId, string? name, string? description, double? minPrice, double? maxPrice, string? sortBy, bool? desc);
        List<MenuItem> GetMenu(Restaurant restaurant);
        MenuItem GetMenuById(int id, Restaurant restaurant);
        Restaurant GetRestaurantById(int id);
        List<Restaurant> GetRestaurants(string? name, string? category, double? minRating, double? maxRating, string? sortBy, bool? desc);
        bool UpdateMenuItem(MenuItem menuItem);
        bool UpdateRestaurant(Restaurant restaurant);
    }
}