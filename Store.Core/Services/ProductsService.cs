using AutoMapper;
using Store.Core.DTO.Product;
using Store.Core.Entities.Product;
using Store.Core.Interfaces;
using Store.Core.Shared;
namespace Store.Core.Services
{
  public class ProductsService : IProductsService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotosService _photosService;

    public ProductsService(IUnitOfWork unitOfWork, IMapper mapper, IPhotosService photosService)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _photosService = photosService;
    }
    public async Task<IEnumerable<ProductDTO>?> GetAllProductsAsync(ProductParams param )
    {

      var products = _unitOfWork.ProductRepository
        .GetAll(x=>x.Category,x=> x.Photos);
      if (products == null)
        return null;

      //filter by word
      if (!string.IsNullOrEmpty(param.Search))
      {
        var searchTerm = param.Search.Split(" ");
        products = products.Where(p => searchTerm.Any(word=>
        p.Name.ToLower().Contains(word.ToLower())||
        p.Description.ToLower().Contains(word.ToLower())
        ));
      }

      //filter by categoryid
      if (param.CategoryId.HasValue )
      {
        products = products.Where(p => p.CategoryId == param.CategoryId);
      }

      //sort 
      products = param.Sort switch
      {
        Enum.SortOptions.ASC => products.OrderBy(p => p.NewPrice),
        Enum.SortOptions.DESC => products.OrderByDescending(p => p.NewPrice),
        _ => products.OrderBy(p => p.Name)
      };

      param.TotatlCount = products.Count();

      products = products
        .Skip((param.PageNumber - 1) * param.pageSize)
        .Take(param.pageSize);

      var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
      return productDTOs;
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
      }
      var product=await _unitOfWork.ProductRepository.GetByIdAsync(id,x=>x.Photos,x=>x.Category);
      var productDTO=_mapper.Map<ProductDTO>(product);
      return productDTO;
    }

    public async Task<ProductDTO?> AddProductAsync(AddProductDTO? productDTO)
    {
      if (productDTO == null)
          return null;
      var entity = _mapper.Map<Product>(productDTO);

      var ImagePath=await _photosService.AddPhotoAsync(productDTO.Photos,productDTO.Name!);

      var product= await _unitOfWork.ProductRepository.AddAsync(entity);
      if (product == null)
          return null;

      var photos = ImagePath.Select(x => new Photo
      {
        ImageName = x,
        ProductId = product.Id
      }).ToList();

      var result = await _unitOfWork.PhotoRepository.AddRangeAsync(photos);
      if (!result)
      {
        throw new Exception("Failed to add photos to the product.");
      }
      var resulte= _mapper.Map<ProductDTO>(product);
      return resulte;
    }

    public async Task<ProductDTO?> UpdateProductAsync(UpdateProductDTO? product)
    {
      if (product == null)
        return null;
      var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id, x => x.Photos, x => x.Category);
      if (existingProduct == null)
        return null;
      var enity=_mapper.Map<Product>(product);

      var findPhotos = await _unitOfWork.PhotoRepository.GetPhotosByProductIdAsync(product.Id);
      if (findPhotos != null)
      {
        foreach (var item in findPhotos)
      { 
        if(item.ImageName == null)
          continue;
        _photosService.DeletePhoto( item.ImageName);
      }
        await _unitOfWork.PhotoRepository.DeleteRangeAsync(findPhotos);
      }
      var ImagePath = await _photosService.AddPhotoAsync(product.Photos, product.Name!);

      var photos = ImagePath.Select(x => new Photo
      {
        ImageName = x,
        ProductId = product.Id
      }).ToList();

      var result = await _unitOfWork.PhotoRepository.AddRangeAsync(photos);
      if (!result)
      {
        throw new Exception("Failed to add photos to the product.");
      }

      var productResult= await _unitOfWork.ProductRepository.UpdateAsync(enity);
      return _mapper.Map<ProductDTO>(productResult);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      var findPhotos = await _unitOfWork.PhotoRepository.GetPhotosByProductIdAsync(id);
      if (findPhotos != null)
      {
        foreach (var item in findPhotos)
        {
          if (item.ImageName == null)
            continue;
          _photosService.DeletePhoto( item.ImageName);
        }
      }
      return await _unitOfWork.ProductRepository.DeleteAsync(id);
    }

    public async Task UpdateProductRatingAsync(int productId)
    {
      var reviews =await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId);

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
    }



  }
}
