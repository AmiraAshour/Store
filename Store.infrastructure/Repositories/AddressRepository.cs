using Store.Core.Entities;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Repositories
{
  public class AddressRepository : IAddressRepository
  {
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Address?> AddAddressAsync(Address address)
    {
      await _context.Addresses.AddAsync(address);
      await _context.SaveChangesAsync();
      return address;
    }

    public async Task<Address?> GetAddressAsync(string email)
    {
      return await _context.Addresses.FindAsync(email).AsTask();
    }

    public async Task<Address?> UpdateAddressAsync(Address address)
    {
      _context.Addresses.Update(address);
      await _context.SaveChangesAsync();
      return address;
    }
  }
}
