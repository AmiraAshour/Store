using Microsoft.EntityFrameworkCore;
using Store.Core.DTO.ProductDTO;
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;

namespace Store.infrastructure.Repositories
{
  public class ProductRepository : GenericReposeitory<Product>, IProductRepository
  {
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) : base(context)
    {
      _context=context;
    }
    public IQueryable<ProductSalesDto> GetTopSellingProductsAsync(int? categoryId=null)
    {

      var query = _context.OrderItems.Include(x=>x.Product).AsQueryable();
      if (categoryId.HasValue)
      {
        query = query.Where(x => x.Product!.CategoryId == categoryId.Value);
      }
      return query
    .GroupBy(oi => oi.Product)
    .Select(g => new ProductSalesDto
    {
      Product = g.Key,                
      TotalSold = g.Sum(x => x.Quntity)
    });
    }


  }
}
