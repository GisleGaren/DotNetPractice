using MyShop.Models;

namespace MyShop.DAL
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetOrderById(int id);
        Task Create(Order order);
        Task Update(Order order);
        Task<bool> Delete(int id);
        Task<Order?> Find(int id);
    }
}
