using MyShop.Models;

namespace MyShop.DAL
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer?> GetCustomerById(int id);
        Task Create(Customer customer);
        Task Update(Customer customer);
        Task<bool> Delete(int id);
        Task<Customer?> Find(int id);
    }
}
