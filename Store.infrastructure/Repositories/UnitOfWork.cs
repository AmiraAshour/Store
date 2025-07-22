

using Store.Core.Interfaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly AppDbContext _context;
    public ICategoryRepository CategoryRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IPhotoRepository PhotoRepository { get; }
    public UnitOfWork( AppDbContext context)
    {
     _context = context;
      CategoryRepository = new CategoryRepository(_context);
      ProductRepository = new ProductRepository(_context);
      PhotoRepository = new PhotoRepository(_context);
    }
  }
}
