using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public class OrderRepository : IOrderRepository
{
    private readonly ItemDbContext _db;

    public OrderRepository(ItemDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        return await _db.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _db.Orders.FindAsync(id);
    }
    public async Task<Order?> Find(int id)
    {
        return await _db.Orders.FindAsync(id);
    }

    public async Task Create(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Order order)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null)
        {
            return false;
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }
}

