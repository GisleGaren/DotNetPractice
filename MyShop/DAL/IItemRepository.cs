using MyShop.Models;
namespace MyShop.DAL
{
    // Remember, that an interface is used to define a set of rules that classes must adhere to. By defining this interface, we are saying
    // that all classes implementing this interface must provide implementations for these methods. This allows us to Abstract the data access
    // logic, making it easier to switch between data storage mechanisms like databases or the dummy data for testing.
    // 
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAll();
        Task<Item?> GetItemById(int id);
        Task Create (Item item);
        Task Update(Item item);
        Task<bool> Delete(int id);

    }
}
