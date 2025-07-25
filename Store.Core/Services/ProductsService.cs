using AutoMapper;
using Store.Core.DTO;
using Store.Core.Entities.Product;
using Store.Core.Interfaces;
namespace Store.Core.Services
{
  public class ProductsService : IProductsService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<Product?> AddProductAsync(ProductDTO? productDTO)
    {
      if (productDTO == null)
      {
        throw new ArgumentNullException(nameof(productDTO), "Product cannot be null.");
      }
      var entity = _mapper.Map<Product>(productDTO);
      if (entity == null)
      {
        throw new InvalidOperationException("Mapping from ProductDTO to Product failed.");
      }
      var product= await _unitOfWork.ProductRepository.AddAsync(entity);
      return product;
    }


    public async Task<bool> DeleteProductAsync(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
      }
      return await _unitOfWork.ProductRepository.DeleteAsync(id);
    }

  
    public async Task<IEnumerable<Product>?> GetAllProductsAsync()
    {
      var products =await _unitOfWork.ProductRepository.GetAllAsync();
     
      return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
      }
      return await _unitOfWork.ProductRepository.GetByIdAsync(id);
    }

    public async Task<Product?> UpdateProductAsync(UpdateProductDTO? product)
    {
      if (product == null)
      {
        throw new ArgumentNullException(nameof(product), "Product cannot be null.");
      }
      var enity=_mapper.Map<Product>(product);
      return await _unitOfWork.ProductRepository.UpdateAsync(enity);
    }
  }
}
