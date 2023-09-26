namespace MyShop.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    // navigation property in an entity class represents a relationship between entities and allows you to navigate from one entity to related entities.
    // In this case, Orders property is a nagivation property that represents the relationship between a Customer and their associated Order entities.
    // By having the Orders attribute here, we can easily see every order of each customer.
    public virtual List<Order>? Orders { get; set; }
}

