using MyShop.Models;

namespace MyShop.DAL
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAll();
        Task<OrderItem?> GetOrderItemById(int id);
        Task Create(OrderItem order);
        Task Update(OrderItem order);
        Task<bool> Delete(int id);
        Task<OrderItem?> Find(int id);
    }
}
