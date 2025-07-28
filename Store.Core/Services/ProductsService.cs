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
    private readonly IPhotosService _photosService;

    public ProductsService(IUnitOfWork unitOfWork, IMapper mapper, IPhotosService photosService)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _photosService = photosService;
    }
    public async Task<IEnumerable<ProductDTO>?> GetAllProductsAsync()
    {
      var products =await _unitOfWork.ProductRepository
        .GetAllAsync(x=>x.Category,x=> x.Photos);
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

      var product= await _unitOfWork.ProductRepository.AddAsync(entity);
      if (product == null)
          return null;

      var ImagePath=await _photosService.AddPhotoAsync(productDTO.Photos,productDTO.Name!);

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
        _photosService.DeletePhoto(Path.Combine("images", item.ImageName));
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
      if (id <= 0)
      {
        throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
      }
      var findPhotos = await _unitOfWork.PhotoRepository.GetPhotosByProductIdAsync(id);
      if (findPhotos != null)
      {
        foreach (var item in findPhotos)
        {
          if (item.ImageName == null)
            continue;
          _photosService.DeletePhoto(Path.Combine("images", item.ImageName));
        }
        await _unitOfWork.PhotoRepository.DeleteRangeAsync(findPhotos);
      }
      return await _unitOfWork.ProductRepository.DeleteAsync(id);
    }

  

  }
}
