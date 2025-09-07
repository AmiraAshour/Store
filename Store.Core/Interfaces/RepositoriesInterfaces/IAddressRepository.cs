using Store.Core.Entities;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IAddressRepository
  {
    Task<List<Address>?> GetAddressesAsync(string userId);
    Task <Address?> GetAddressById(int id); 
    Task<Address?> UpdateAddressAsync(Address address);
    Task<Address?> AddAddressAsync(Address address);
    Task<bool> DeleteAddressAsync(int id);
  }
}
