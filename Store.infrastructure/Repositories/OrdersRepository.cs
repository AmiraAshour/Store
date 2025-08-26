using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class OrdersRepository : IOrdersRepository
  {
    private readonly AppDbContext _context;

    public OrdersRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Orders> AddOrederAsync(Orders oreder)
    {
      await _context.Orders.AddAsync(oreder);
     await _context.SaveChangesAsync();
      return oreder;
    }

  
    public async Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail)
    {
      return await _context.Orders.Where(o => o.BuyerEmail == BuyerEmail).ToListAsync();
    }
    public async Task<Orders?> GetOrderByIdAsync(int Id, string BuyerEmail)
    {
       return await _context.Orders.FirstOrDefaultAsync(o => o.Id == Id && o.BuyerEmail == BuyerEmail);
       
    }
  }
}
