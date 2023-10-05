using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        // This creates a scope, which is a way to manage lifetime and disposal of services within a specific context.
        // A scope is like a library bookshelf where each bookshelf (scope) contains a specific category of books (services). So the scope is just
        // a temporary workspace which we create to work on certain tasks. Scopes are important because they manage lifespan of resources and improve
        // resource utilization by limiting what's loaded into memory.
        using var serviceScope = app.ApplicationServices.CreateScope();
        // When the context object is instantiated below, it's like going to the library staff (service provider) at the right bookshelf (scope) 
        // and saying "I need the ItemDbContext book (service) to work with" The library staff (service provider) proivdes you with a specific instance
        // of ItemDbContext to work with in that specific HTTP request, ensunring that we have an isolated work space for each request.
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        // If we want to not delete the existing database and create from scratch, we can comment out EnsureDeleted()

        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Checks if there any any elements in the "Items" table, and if not, we add initial data to the database if empty.
        if (!context.Items.Any())
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Pizza",
                    Price = 150,
                    Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
                    ImageUrl = "/images/pizza.jpg"
                },
                new Item
                {
                    Name = "Fried Chicken Leg",
                    Price = 20,
                    Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
                    ImageUrl = "/images/chickenleg.jpg"
                },
                new Item
                {
                    Name = "French Fries",
                    Price = 50,
                    Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
                    ImageUrl = "/images/frenchfries.jpg"
                },
                new Item
                {
                    Name = "Grilled Ribs",
                    Price = 250,
                    Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
                    ImageUrl = "/images/ribs.jpg"
                },
                new Item
                {
                    Name = "Tacos",
                    Price = 150,
                    Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
                    ImageUrl = "/images/tacos.jpg"
                },
                new Item
                {
                    Name = "Fish and Chips",
                    Price = 180,
                    Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
                    ImageUrl = "/images/fishandchips.jpg"
                },
                new Item
                {
                    Name = "Cider",
                    Price = 50,
                    Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
                    ImageUrl = "/images/cider.jpg"
                },
                new Item
                {
                    Name = "Coke",
                    Price = 30,
                    Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
                    ImageUrl = "/images/coke.jpg"
                },
            };
            // Adds list of items to the Items DbSet within the database context and prepares these items to be inserted into the database.
            context.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Alice Hansen", Address = "Osloveien 1"},
                new Customer { Name = "Bob Johansen", Address = "Oslomet gata 2"},
            };
            context.AddRange(customers);
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 1,},
                new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 2,},
            };
            context.AddRange(orders);
            context.SaveChanges();
        }

        if (!context.OrderItems.Any())
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { ItemId = 1, Quantity = 2, OrderId = 1},
                new OrderItem { ItemId = 2, Quantity = 1, OrderId = 1},
                new OrderItem { ItemId = 3, Quantity = 4, OrderId = 2},
            };
            foreach (var orderItem in orderItems)
            {
                // Looks up Item associated with current OrderItem based on ItemId
                var item = context.Items.Find(orderItem.ItemId);
                // Calculates the OrderItemPrice for the current OrderItem by multiplying Quantity with Price and if item is null,
                // we set the OrderItemPrice to 0, otherwise if not null, we calculate the OrderItemPrice.
                orderItem.OrderItemPrice = orderItem.Quantity * item?.Price ?? 0;
            }
            context.AddRange(orderItems);
            context.SaveChanges();
        }

        // Fetch all the Order entities from the database and include all the related OrderItems for each order. Here we have utilized eager loading,
        // as opposed to easy loading! We have this because we added the .Include() method. We do this so that when we acces an OrderItems later in
        // the loop, they are already loaded in the memory, reducing database queries.
        var ordersToUpdate = context.Orders.Include(o => o.OrderItems);
        // Now we want to update TotalPrice attribute in Order.cs and order.OrderItems represents the collection of OrderItems entities connected to each
        // order and since we used eager loading, all of these are already loaded.
        foreach (var order in ordersToUpdate)
        {
            // Calculate the sum of the OrderItemPrice attribute of each item in the OrderItems collection and if order has no items (null)
            // simply return 0.
            order.TotalPrice = order.OrderItems?.Sum(oi => oi.OrderItemPrice) ?? 0;
        }
        context.SaveChanges();
    }
}
