﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DAL;
using MyShop.Models;
using MyShop.ViewModels;

namespace MyShop.Controllers;

public class ItemController : Controller
{
    // This time we are adding the ItemRepository interface _itemRepository, which contains all the methods that are in DAL (data access layer) class

    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemController> _logger;
    // Our new constructor is going to initialize the IItemRepository Interface to use it within our Action methods
    public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Table()
    {
        // Same method with async, which makes this I/O call to the database work in the background to not block other processes.
        // instead of await _itemDbContext.Items.ToListAsync(), we can simply just call the itemRepository.GetAll() method.
        var items = await _itemRepository.GetAll();
        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found");
        }
        var itemListViewModel = new ItemListViewModel(items, "Table");
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        var items = await _itemRepository.GetAll();
        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found");
        }
        var itemListViewModel = new ItemListViewModel(items, "Grid");
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return NotFound("Item not found for the ItemId");
        }
        return View(item);
    }

    // We use HttpGet for retrieval of websites using a URL
    [HttpGet]
    // Authorize decorators are used so that users are not able to access all pages without being logged
    // We add the decorator to all methods that change the database.
    // Once we run the project, we can still view all the pages, but if we try to change the database, the browser will automatically jump to the login page
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Item item)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _itemRepository.Create(item);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ItemController] Item creation failed {@item}", item);
        return View(item);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found when updating the ItemId {ItemId:0000}", id);
            return BadRequest("Item not found for the ItemId");
        }
        return View(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(Item item)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _itemRepository.Update(item);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ItemController] Item update failed {@item}", item);
        return View(item);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return BadRequest("Item not found for the ItemId");
        }
        return View(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _itemRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
            return BadRequest("Item deletion failed");
        }
        return RedirectToAction(nameof(Table));
    }
}
