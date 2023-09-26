using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using Microsoft.EntityFrameworkCore;
using MyShop.ViewModels;
// New dependency to render the dropdown menu.
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DAL;

namespace MyShop.Controllers;

public class OrderController : Controller
{
    private readonly ItemDbContext _itemDbContext;

    public OrderController(ItemDbContext itemDbContext)
    {
        _itemDbContext = itemDbContext;
    }

    public async Task<IActionResult> Table()
    {
        // With the line below, we first query _itemDbContext, which in this case is a database table and we call ToListAsync() to retrieve all items 
        // from the table as a list of Item objects.
        List<Order> orders = await _itemDbContext.Orders.ToListAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOrderItem()
    {
        // We query the database again to list all the items and orders that we can get and we need to async these to make these processes run
        // in the background because if not, these are blocking calls that take up a lot of time.
        var items = await _itemDbContext.Items.ToListAsync();
        var orders = await _itemDbContext.Orders.ToListAsync();
        var createOrderItemViewModel = new CreateOrderItemViewModel
        {
            OrderItem = new OrderItem(),

            // Transform each item in the items list into a new SelectListItem object and each Value attribute gets the ItemID of that object.
            // Each Text attribute in this new object will have the item id and the item name next to it.
            ItemSelectList = items.Select(item => new SelectListItem
            {
                Value = item.ItemId.ToString(),
                Text = item.ItemId.ToString() + ": " + item.Name
            }).ToList(),
            OrderSelectList = orders.Select(order => new SelectListItem
            {
                Value = order.OrderId.ToString(),
                Text = "Order" + order.OrderId.ToString() + ", Date: " + order.OrderDate + ", Customer: " + order.Customer.Name
            }).ToList(),
        };
        return View(createOrderItemViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItem orderItem)
    {
        try
        {
            var newItem = _itemDbContext.Items.Find(orderItem.ItemId);
            var newOrder = _itemDbContext.Orders.Find(orderItem.OrderId);

            if (newItem == null || newOrder == null)
            {
                return BadRequest("Item or Order not found.");
            }

            var newOrderItem = new OrderItem
            {
                ItemId = orderItem.ItemId,
                Item = newItem,
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                Order = newOrder,
            };
            newOrderItem.OrderItemPrice = orderItem.Quantity * newOrderItem.Item.Price;

            _itemDbContext.OrderItems.Add(newOrderItem);
            await _itemDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        catch
        {
            return BadRequest("OrderItem creation failed.");
        }
    }
}
