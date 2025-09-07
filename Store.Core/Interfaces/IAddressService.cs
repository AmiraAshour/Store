

using Store.Core.DTO.Account;
using Store.Core.Entities;

namespace Store.Core.Interfaces
{
  public interface IAddressService
  {
    Task<List<RetriveAddressDTO>?> GetAllAddressesAsync(string userId);
    Task<Address?> GetAddressByIdAsync(int id);
    Task<RetriveAddressDTO?> AddAddressAsync(AddressDTO address,string userId);
    Task<RetriveAddressDTO?> UpdateAddressAsync(int addresId, AddressDTO addressDto, string userId);
    Task<bool> DeleteAddressAsync(int id,string userId);
  }
}
