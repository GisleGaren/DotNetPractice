using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using MyShop.ViewModels;

namespace MyShop.Controllers;

public class ItemController : Controller
{
    // For the construction injector, this is just a dependency that we need to inject into the ItemController.cs class in order for us
    // to interact with the database and manipulate the data in the database.
    private readonly ItemDbContext _itemDbContext;

    // The constructor is called when an ItemController instance is created, typically when handling an incoming HTTP request (When the Views
    // in the ItemController are called such as Table(), Grid() or Details()
    // ASP.NET handles the instantiation of the controller and its dependencies through dependency injection.
    public ItemController(ItemDbContext itemDbContext)
    {
        _itemDbContext = itemDbContext;
    }

    // ADDED ASYNC which enables a time consuming task to run in the background without blocking other processes and it has to return Task<T>
    public async Task<IActionResult> Table()
    {
        // With the line below, we first query _itemDbContext, which in this case is a database table and we call ToList() to retrieve all items 
        // from the table as a list of Item objects.
        List<Item> items = await _itemDbContext.Items.ToListAsync();
        var itemListViewModel = new ItemListViewModel(items, "Table");
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        List<Item> items = await _itemDbContext.Items.ToListAsync(); 
        var itemListViewModel = new ItemListViewModel(items, "Grid"); // Here instead, we have the list and String into the ItemListViewModel to have several returns.
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        // In this details method, it takes in a specific id as an argument and much like React with the .map() method, we are using a lambda expression.
        // List<Item> items = await _itemDbContext.Items.ToListAsync();
        var item = await _itemDbContext.Items.FirstOrDefaultAsync(i => i.ItemId == id); // FirstorDefault is a LINQ method to find the first item in the array that matches the id from the argument
        if(item == null) { // If found, we assign it to the var item variable, if item == null, that means the id does not exist in the array
            return NotFound();  // and we return the NotFound() method, which produces an HTTP 404 not found response.
        }
        return View(item);
    }
    // We use this action method to display the form that we want to create. When a user navigates to the Create view, we invoke the GET request.
    // This GET request is associated with the /Create url route and once the user has gone to the /Create url, we return the Create View.
    // We use this GET tag to decorate methods in order to retrieve data from the server.
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Post method that we use to handle submission of the form when we have filled it and it takes an item object as parameter which is bound by the form data
    // sent in the Create View. This method checks if the form data passed the validation rules. If valid, we then add the form info as a row into the database
    // and the changes are then saved (commited) using the SaveChanges() method. After successfully adding the item to the database, we direct to the Table View.
    // If invalid, we just return the same Create View with an error message.
    [HttpPost]
    public async Task<IActionResult> Create(Item item) 
    {
        if(ModelState.IsValid)
        {
            // the .Add() method tells Entity Framework to consider "item" argument as a new entity that should be inserted into the database when we call
            // saveChanges(). Entity Frameworks keeps track of the change in memory, but doesn't immediately do database commands / manipulations.
            _itemDbContext.Items.Add(item);
            await _itemDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        return View(item);
    }

    // This is used to display the form for the specific item id that we want to update. It has the GET attribute that indicates that it handles HTTP GET requests
    // When a user navigates to the Update page, it invokes this method and it takes the id parameter to identify the item to be updated.
    [HttpGet]
    public async Task<IActionResult> Update(int id) {
        var item = await _itemDbContext.Items.FindAsync(id); // Retrives the item using the Find method from the database with the given ID.
        if(item == null)
        {
            return NotFound(); // If not found, return 404 error.
        }
        return View(item); // If found, it returns the Update View page.
    }

    // This one is a post action method that handles the HTTP Post request to process the updated data form submission the Update View into the database.
    // We take the item object representing the updated data submitted through the form in the Update View.
    [HttpPost]
    public async Task<IActionResult> Update(Item item)
    {
        if(ModelState.IsValid) // We then check if the data passed passes the validation as defined in the Item Model class
        {
            // Much like .Add(), Update() works with the Entity Framework tracker and marks the changes as updated and doesn't involve I/O or database communication.
            _itemDbContext.Items.Update(item); // If valid, we update the database using Update() and we save the changes
            await _itemDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table)); // If everything successful, we then redirect to the Table View.
        }
        // If it fails, we return a view of the same page with errors instead of the Table view above within the modelState.IsValid page.
        return View(item); // 
    }

    // Get method to display the confirmation page for the delete button, if not found, we return 404 not found.
    [HttpGet]
    public async Task<IActionResult> Delete(int id) 
    {
        var item = await _itemDbContext.Items.FindAsync(id); // Again, we find the item id we want to delete and return the object
        if(item == null)
        {
            return NotFound(); // return 404 not found
        }
        return View(item); // Else redirect to the confirmation delete page.
    }

    // Post action method to actually delete the item from the list.
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _itemDbContext.Items.FindAsync(id); // Takes in the item id and finds it in the database.
        if(item == null)
        {
            return NotFound();      // return 404 not found if not found
        }
        // Same with the Remove() method, is works with the entity framework change tracker, marking the entity for deletion in memory. Whilst the actual
        // deleting goes on in the SaveChangesAsync() method.
        _itemDbContext.Items.Remove(item);      // remove the item from the database if found.
        await _itemDbContext.SaveChangesAsync();           // Then we save the changes in the database
        return RedirectToAction(nameof(Table));     // Redirect to the table View to see that the item has now been deleted.
    }

    //public IActionResult Table()
    //{
    //    var items = GetItems();
    //    ViewBag.CurrentViewName = "Table";
    //    return View(items);
    //}

    //public IActionResult Grid()
    //{
    //    var items = GetItems();
    //    ViewBag.CurrentViewName = "Grid"; // This will return "Grid" in the "@ViewBag.CurrentViewName View" when the Grid View is being utilized
    //    return View(items); 
    //}

    public List<Item> GetItems()
    {
        var items = new List<Item>();
        var item1 = new Item
        {
            ItemId = 1,
            Name = "Pizza",
            Price = 150,
            Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
            ImageUrl = "/images/pizza.jpg"
        };

        var item2 = new Item
        {
            ItemId = 2,
            Name = "Fried Chicken Leg",
            Price = 20,
            Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
            ImageUrl = "/images/chickenleg.jpg"
        };

        var item3 = new Item
        {
            ItemId = 3,
            Name = "French Fries",
            Price = 50,
            Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
            ImageUrl = "/images/frenchfries.jpg"
        };

        var item4 = new Item
        {
            ItemId = 4,
            Name = "Grilled Ribs",
            Price = 250,
            Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
            ImageUrl = "/images/ribs.jpg"
        };

        var item5 = new Item
        {
            ItemId = 5,
            Name = "Tacos",
            Price = 150,
            Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
            ImageUrl = "/images/tacos.jpg"
        };

        var item6 = new Item
        {
            ItemId = 6,
            Name = "Fish and Chips",
            Price = 180,
            Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
            ImageUrl = "/images/fishandchips.jpg"
        };

        var item7 = new Item
        {
            ItemId = 7,
            Name = "Cider",
            Price = 50,
            Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
            ImageUrl = "/images/cider.jpg"
        };

        var item8 = new Item
        {
            ItemId = 8,
            Name = "Coke",
            Price = 30,
            Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
            ImageUrl = "/images/coke.jpg"
        };


        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        items.Add(item4);
        items.Add(item5);
        items.Add(item6);
        items.Add(item7);
        items.Add(item8);
        return items;
    }
}


/* old way of doing this controller class
public class ItemController : Controller
{
    public IActionResult Table()
    {
        var items = new List<Item>();
        var item1 = new Item();
        item1.ItemId = 1;
        item1.Name = "Pizza";
        item1.Price = 60;

        var item2 = new Item
        {
            ItemId = 2,
            Name = "Fried Chicken Leg",
            Price = 15
        };

        items.Add(item1);
        items.Add(item2);

        ViewBag.CurrentViewName = "List of Shop Items";
        // Passes the items array into a view and the data
        return View(items);
    }
} */