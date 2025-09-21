using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Order;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.infrastructure.Data;
using System.Linq.Expressions;

namespace Store.infrastructure.Repositories
{
  public class OrdersRepository : IOrdersRepository
  {
    private readonly AppDbContext _context;

    public OrdersRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Orders>> GetAllOrdersAsync(Expression<Func<Orders, bool>> filter)
    {
      return await _context.Orders.Where(filter).ToListAsync();
    }
    public async Task<Orders> AddOrederAsync(Orders oreder)
    {
      await _context.Orders.AddAsync(oreder);
      await _context.SaveChangesAsync();
      return oreder;
    }


    public async Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail)
    {
      return await _context.Orders.
        Where(o => o.BuyerEmail == BuyerEmail).
        Include(x => x.deliveryMethod).
        Include(x => x.orderItems).
        ToListAsync();
    }
    public async Task<Orders?> GetOrderByIdAsync(int Id)
    {
      return await _context.Orders.
       Where(o => o.Id == Id).
       Include(x => x.deliveryMethod).
       Include(x => x.orderItems).
       FirstOrDefaultAsync();

    }

    public async Task<Orders?> UpdateOrderAsync(Orders oreder)
    {
      _context.Update(oreder);
      await _context.SaveChangesAsync();
      return oreder;
    }

   
    public async Task<bool> HasPurchased(string buyerEmail, int productId)
    {
      return await _context.Orders
         .Include(o => o.orderItems)
         .AnyAsync(o => o.BuyerEmail == buyerEmail &&
                        o.orderItems.Any(i => i.ProductItemId == productId) && 
                        o.status == Status.PaymentReceived);
    }
  }
}
