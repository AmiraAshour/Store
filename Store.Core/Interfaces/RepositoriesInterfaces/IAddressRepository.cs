using Store.Core.Entities;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IAddressRepository
  {
    Task<Address?> GetAddressAsync(string email);
    Task<Address?> UpdateAddressAsync(Address address);
    Task<Address?> AddAddressAsync(Address address);
  }
}
