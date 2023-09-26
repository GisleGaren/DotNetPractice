using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ItemDbContext _db;

        public CustomerRepository(ItemDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _db.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            return await _db.Customers.FindAsync(id);
        }

        public async Task Create(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<Customer?> Find(int id)
        {
            return await _db.Customers.FindAsync(id);
        }
    }
}
