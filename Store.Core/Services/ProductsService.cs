using AutoMapper;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Core.DTO.Product;
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces;
using Store.Core.Shared;

namespace Store.Core.Services
{
  public class ProductsService : IProductsService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotosService _photosService;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IUnitOfWork unitOfWork, IMapper mapper, IPhotosService photosService, ILogger<ProductsService> logger)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _photosService = photosService;
      _logger = logger;
    }

    public async Task<IEnumerable<ProductDTO>?> GetAllProductsAsync(ProductParams param)
    {
      _logger.LogInformation("Fetching all products with filter: {@Params}", param);

      var products = _unitOfWork.ProductRepository
          .GetAll(x => x.Category, x => x.Photos);

      if (products == null)
      {
        _logger.LogWarning("No products found.");
        return null;
      }

      if (!string.IsNullOrEmpty(param.Search))
      {
        _logger.LogInformation("Applying search filter: {Search}", param.Search);

        products = products.Where(p =>
            EF.Functions.FreeText(p.Name!, param.Search) ||
            EF.Functions.FreeText(p.Description!, param.Search)
        );
      }

      if (param.CategoryId.HasValue)
      {
        _logger.LogInformation("Filtering by CategoryId: {CategoryId}", param.CategoryId);
        products = products.Where(p => p.CategoryId == param.CategoryId);
      }

      products = param.Sort switch
      {
        Enum.SortOptions.ASC => products.OrderBy(p => p.NewPrice),
        Enum.SortOptions.DESC => products.OrderByDescending(p => p.NewPrice),
        _ => products.OrderBy(p => p.Name)
      };

      param.TotatlCount = products.Count();
      _logger.LogInformation("Total products after filtering: {Count}", param.TotatlCount);

      products = products
          .Skip((param.PageNumber - 1) * param.pageSize)
          .Take(param.pageSize);

      return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
      _logger.LogInformation("Fetching product with Id {Id}", id);

      if (id <= 0)
      {
        _logger.LogError("Invalid product Id: {Id}", id);
        throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
      }

      var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, x => x.Photos, x => x.Category);
      if (product == null)
      {
        _logger.LogWarning("Product not found with Id {Id}", id);
        return null;
      }

      return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO?> AddProductAsync(AddProductDTO? productDTO)
    {
      if (productDTO == null)
      {
        _logger.LogWarning("Attempted to add null product.");
        return null;
      }

      _logger.LogInformation("Adding new product: {Name}", productDTO.Name);

      var entity = _mapper.Map<Product>(productDTO);
      var imagePaths = await _photosService.AddPhotoAsync(productDTO.Photos, productDTO.Name!);

      var product = await _unitOfWork.ProductRepository.AddAsync(entity);
      if (product == null)
      {
        _logger.LogError("Failed to add product: {Name}", productDTO.Name);
        return null;
      }

      var photos = imagePaths.Select(x => new Photo
      {
        ImageName = x,
        ProductId = product.Id
      }).ToList();

      var result = await _unitOfWork.PhotoRepository.AddRangeAsync(photos);
      if (!result)
      {
        _logger.LogError("Failed to add photos for product Id {ProductId}", product.Id);
        throw new Exception("Failed to add photos to the product.");
      }

      _logger.LogInformation("Product added successfully with Id {Id}", product.Id);
      return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO?> UpdateProductAsync(UpdateProductDTO? product)
    {
      if (product == null)
      {
        _logger.LogWarning("Attempted to update null product.");
        return null;
      }

      _logger.LogInformation("Updating product with Id {Id}", product.Id);

      var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id, x => x.Photos, x => x.Category);
      if (existingProduct == null)
      {
        _logger.LogWarning("Product not found for update, Id {Id}", product.Id);
        return null;
      }

      var entity = _mapper.Map<Product>(product);

      var findPhotos = await _unitOfWork.PhotoRepository.GetPhotosByProductIdAsync(product.Id);
      if (findPhotos != null)
      {
        foreach (var item in findPhotos)
        {
          if (item.ImageName == null) continue;
          _photosService.DeletePhoto(item.ImageName);
          _logger.LogInformation("Deleted old photo: {Photo}", item.ImageName);
        }

        await _unitOfWork.PhotoRepository.DeleteRangeAsync(findPhotos);
      }

      var imagePaths = await _photosService.AddPhotoAsync(product.Photos, product.Name!);
      var photos = imagePaths.Select(x => new Photo
      {
        ImageName = x,
        ProductId = product.Id
      }).ToList();

      var result = await _unitOfWork.PhotoRepository.AddRangeAsync(photos);
      if (!result)
      {
        _logger.LogError("Failed to add new photos for product Id {Id}", product.Id);
        throw new Exception("Failed to add photos to the product.");
      }

      var productResult = await _unitOfWork.ProductRepository.UpdateAsync(entity);
      _logger.LogInformation("Product updated successfully with Id {Id}", product.Id);

      return _mapper.Map<ProductDTO>(productResult);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      _logger.LogInformation("Deleting product with Id {Id}", id);

      var findPhotos = await _unitOfWork.PhotoRepository.GetPhotosByProductIdAsync(id);
      if (findPhotos != null)
      {
        foreach (var item in findPhotos)
        {
          if (item.ImageName == null) continue;
          _photosService.DeletePhoto(item.ImageName);
          _logger.LogInformation("Deleted photo {Photo} for product {Id}", item.ImageName, id);
        }
      }

      var result = await _unitOfWork.ProductRepository.DeleteAsync(id);
      if (result)
        _logger.LogInformation("Product deleted successfully with Id {Id}", id);
      else
        _logger.LogWarning("Failed to delete product with Id {Id}", id);

      return result;
    }

    public async Task UpdateProductRatingAsync(int productId)
    {
      _logger.LogInformation("Updating rating for product Id {Id}", productId);

      var reviews = await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId);
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

      if (reviews.Any())
      {
        product.AverageRating = reviews.Average(r => r.Rating);
        product.ReviewCount = reviews.Count();
      }
      else
      {
        product.AverageRating = 0;
        product.ReviewCount = 0;
      }

      await _unitOfWork.ProductRepository.UpdateAsync(product);
      _logger.LogInformation("Product rating updated: {Rating} ({Count} reviews)", product.AverageRating, product.ReviewCount);
    }
  }
}
