using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ItemDbContext _db;

        public OrderItemRepository(ItemDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<OrderItem>> GetAll()
        {
            return await _db.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemById(int id)
        {
            return await _db.OrderItems.FindAsync(id);
        }

        public async Task Create(OrderItem orderItem)
        {
            _db.OrderItems.Add(orderItem);
            await _db.SaveChangesAsync();
        }

        public async Task Update(OrderItem orderItem)
        {
            _db.OrderItems.Update(orderItem);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var orderItem = await _db.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return false;
            }

            _db.OrderItems.Remove(orderItem);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<OrderItem?> Find(int id)
        {
            return await _db.OrderItems.FindAsync(id);
        }
    }
}
