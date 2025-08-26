

using StackExchange.Redis;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly AppDbContext _context;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IPhotoRepository PhotoRepository { get; }
    public IBasketRepository BasketRepository { get; }
    public IDeliveryMethodRepository DeliveryMethodRepository { get; }
    public IOrdersRepository OrdersRepository { get; }
    public IAddressRepository AddressRepository { get; }

    public UnitOfWork(AppDbContext context, IConnectionMultiplexer redis )
    {
      _context = context;
      CategoryRepository = new CategoryRepository(_context);
      ProductRepository = new ProductRepository(_context);
      PhotoRepository = new PhotoRepository(_context);
      BasketRepository = new BasketRepository(redis);
      DeliveryMethodRepository = new DeliveryMethodRepository(_context);
      OrdersRepository=new OrdersRepository(_context);
      AddressRepository = new AddressRepository(_context);
    }
  }
}
