# OnlineFoodMinimalAPI

This project is an **Online Food Management Minimal API** built with **ASP.NET Core 9**. It is part of my **learning journey** into ASP.NET Core and web development. The project focuses on managing restaurants and their menus, including features like CRUD operations, filtering, and sorting.

---

## 🌟 Features

### Restaurants

* Get all restaurants, with optional filtering and sorting:

  * Filter by `name`, `category`, `minRating`, `maxRating`
  * Sort by `name`, `rating`, or `category` (ascending/descending)
* Get a restaurant by ID
* Add a new restaurant
* Update an existing restaurant
* Delete a restaurant

### Menu Items

* Get all menu items for a restaurant
* Get a menu item by ID
* Add a new menu item to a restaurant
* Update a menu item
* Delete a menu item
* Filtering and sorting menu items by `name`, `description`, `price`

---

## 🛠️ Technologies Used

* **.NET 9**
* **C# 12**
* **ASP.NET Core Minimal APIs**
* **In-memory data storage** (using `List<T>`)
* **Model validation** with `[Required]`, `[Range]`, `[MaxLength]`

---

## 🚀 Endpoints Overview

### Restaurants

| Method | Endpoint            | Description                                         |
| ------ | ------------------- | --------------------------------------------------- |
| GET    | `/restaurants`      | List all restaurants (supports filtering & sorting) |
| GET    | `/restaurants/{id}` | Get a specific restaurant                           |
| POST   | `/restaurants`      | Add a new restaurant                                |
| PUT    | `/restaurants/{id}` | Update a restaurant                                 |
| DELETE | `/restaurants/{id}` | Delete a restaurant                                 |

### Menus

| Method | Endpoint                              | Description                                          |
| ------ | ------------------------------------- | ---------------------------------------------------- |
| GET    | `/restaurants/{id}/menu`              | Get all menu items for a restaurant                  |
| GET    | `/restaurants/{id}/menu/{menuItemId}` | Get a specific menu item                             |
| POST   | `/restaurants/{id}/menu`              | Add a new menu item to a restaurant                  |
| PUT    | `/restaurants/{id}/menu/{menuItemId}` | Update a menu item                                   |
| DELETE | `/restaurants/{id}/menu/{menuItemId}` | Delete a menu item                                   |
| GET    | `/restaurants/menus`                  | Get all menu items with optional filtering & sorting |

---

## 📚 Learning Journey Notes

This project was created to **practice ASP.NET Core Minimal APIs** and understand:

* Routing and HTTP methods (`GET`, `POST`, `PUT`, `DELETE`)
* Query parameters and model binding
* In-memory data manipulation using LINQ
* Model validation with data annotations
* Filtering and sorting of data collections
* Structuring code for maintainability
---
