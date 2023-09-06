using System;
using System.Collections.Generic;
using MyShop.Models;

// We add this class and folder because to always have items and CurrentViewName is impractical as we may want to pass data into the View.
// A good practice is to wrap all data needed in a View to a ViewModel.
// With the code below, we can comment out the original code in the ItemController. 
// A View Model is A data structure that wraps all data relevant to the View into an object
namespace MyShop.ViewModels
{
    public class ItemListViewModel
    {
        // Enumerable is an interface in the .NET framework that represents a collection of objects that can be enumerated (iterated).
        // In other words, IEunumerable in our Items attribute means that the attribute promises to provide a way to iterate through all its elements.
        public IEnumerable<Item> Items;
        // Remember that with a question mark after the data type, that means that we are allowing null values.
        public string? CurrentViewName;

        public ItemListViewModel(IEnumerable<Item> items, string? currentViewName)
        {
            Items = items;
            CurrentViewName = currentViewName;
        }
    }
}

