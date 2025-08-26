using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Repositories
{
  public class DeliveryMethodRepository : IDeliveryMethodRepository
  {
    private readonly AppDbContext _context;

    public DeliveryMethodRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<DeliveryMethod?> GetByIdAsync(int id)
    {
      return await _context.deliveryMethods.FirstOrDefaultAsync(x=>x.Id==id);
    }

    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodsAsync()
    {
      return await _context.deliveryMethods.ToListAsync();
    }
  }
}
