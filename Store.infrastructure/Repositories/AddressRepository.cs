using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class AddressRepository : IAddressRepository
  {
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<List<Address>?> GetAddressesAsync(string userId)
    {
      return await _context.Addresses.Where(x=>x.AppUserId==userId).ToListAsync();
    }
    public async Task<Address?> GetAddressById(int id)
    {
      return await _context.Addresses.FindAsync(id);
    }

    public async Task<Address?> AddAddressAsync(Address address)
    {
      await _context.Addresses.AddAsync(address);
      await _context.SaveChangesAsync();
      return address;
    }


    public async Task<Address?> UpdateAddressAsync(Address address)
    {
      _context.Addresses.Update(address);
      await _context.SaveChangesAsync();
      return address;
    }
    public async Task<bool> DeleteAddressAsync(int id) {  
      var address = await _context.Addresses.FindAsync(id);
      if (address is null)
        return false;
      _context.Addresses.Remove(address);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}
