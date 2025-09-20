using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;
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
      return await _context.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodsAsync()
    {
      return await _context.DeliveryMethods.ToListAsync();
    }
    public async Task AddAsync(DeliveryMethod method)
    {
      await _context.DeliveryMethods.AddAsync(method);
      await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(DeliveryMethod method)
    {
      _context.DeliveryMethods.Update(method);
      await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(DeliveryMethod method)
    {
        _context.DeliveryMethods.Remove(method);
        await _context.SaveChangesAsync();
     
    }

   
  }
}
