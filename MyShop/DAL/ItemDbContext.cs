﻿using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// Above after we have downloaded MS.EntityFrameworkCore.SQLite
namespace MyShop.DAL
{
    public class ItemDbContext : IdentityDbContext
    {
        // The DbContextOptions<T> parameter encapsulates various configuration options and settings related to how the database context should operate
        // So once we instantiate the ItemDbContext class, we pass an instance of DbContextOptions<ItemDbContext> class to the constructor so that 
        // Entity Framework Core knows how to configure the database context.

        // An example on how we can configure is by adding a database connection string, which specifies the database server, name, authentication details etc
        // Configuration is usually done through the dependency injector container, which is usually startup.cs
        // The constructor usually includes a database connection string, which represents and establishes the connection to and from the database.
        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        {
            // Below means that we create a database in case it doesn't exist. UPDATE we commented it out because of database migration.
            // Database.EnsureCreated();
        }
        // Below represents a database table named "Items" that allows us to perform CRUD operations
        // on "Items" table using Entity Frameworks DbSet<Item> provides an interface to query and manipulate "Items" table
        // Think of this like each DbSet is a table in the database and each instance of DbSet of type <Item> corresponds to a single row.
        // Any changes we make in DbSet is reflected on the actual database itself and Entity FrameWork Core uses Object Relational Mapping (ORM) to map
        // the Item instance to the database. This is a Code First approach!
        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // The code below enables Lazy Loading by adding it as a dependency injection.
        // By using UseLazyLoadingProxies() on OnConfiguring(), EF Core generates proxies for entity classes, allowing lazy loading of related entities when accessed.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
