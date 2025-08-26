using AutoMapper;
using Store.Core.DTO.Category;
using Store.Core.Entities.Product;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class CategoriesService : ICategoriesService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
  
    public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
     
    }
    public async Task<Category?> AddCategoryAsync(CategoryDTO categoryDto)
    {
      var entity = _mapper.Map<Category>(categoryDto);
      var result = await _unitOfWork.CategoryRepository.AddAsync(entity);
      return result;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      return await _unitOfWork.CategoryRepository.DeleteAsync(id);
    }
    public async Task<Category?> UpdateCategoryAsync(UpdateCategoryDTO categoryDto)
    {
      var existing = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);
      if (existing is null) return null;

      return await _unitOfWork.CategoryRepository.UpdateAsync(existing);
    }
    public IEnumerable<Category> GetAllCategory()
    {
      return  _unitOfWork.CategoryRepository.GetAll().ToList();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("Category ID must be greater than zero.", nameof(id));
      }
      return await _unitOfWork.CategoryRepository.GetByIdAsync(id);
    }
  }
}
