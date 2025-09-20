using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Core.DTO.CategoryEntityDTO;
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.Core.Services
{
  public class CategoriesService : ICategoriesService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoriesService> logger)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<Category?> AddCategoryAsync(CategoryDTO categoryDto)
    {
      _logger.LogInformation("Attempting to add new category: {Name}", categoryDto.Name);

      var entity = _mapper.Map<Category>(categoryDto);
      var result = await _unitOfWork.CategoryRepository.AddAsync(entity);

      if (result != null)
        _logger.LogInformation("✅ Category added successfully with ID {Id}", result.Id);
      else
        _logger.LogWarning("⚠️ Failed to add category: {Name}", categoryDto.Name);

      return result;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      _logger.LogInformation("Attempting to delete category with ID {Id}", id);

      var success = await _unitOfWork.CategoryRepository.DeleteAsync(id);

      if (success)
        _logger.LogInformation("✅ Category with ID {Id} deleted successfully", id);
      else
        _logger.LogWarning("⚠️ Category with ID {Id} could not be deleted", id);

      return success;
    }

    public async Task<Category?> UpdateCategoryAsync(UpdateCategoryDTO categoryDto)
    {
      _logger.LogInformation("Attempting to update category with ID {Id}", categoryDto.Id);

      var existing = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);
      if (existing is null)
      {
        _logger.LogWarning("⚠️ Category with ID {Id} not found for update", categoryDto.Id);
        return null;
      }

      existing.Name = categoryDto.Name ?? existing.Name;
      existing.Description = categoryDto.Description ?? existing.Description;

      var updated = await _unitOfWork.CategoryRepository.UpdateAsync(existing);

      _logger.LogInformation("✅ Category with ID {Id} updated successfully", updated.Id);

      return updated;
    }

    public IEnumerable<Category> GetAllCategory()
    {
      _logger.LogInformation("Fetching all categories");
      var categories = _unitOfWork.CategoryRepository.GetAll().ToList();
      _logger.LogInformation("✅ Retrieved {Count} categories", categories.Count);
      return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
      if (id <= 0)
      {
        _logger.LogError("❌ Invalid category ID: {Id}", id);
        throw new ArgumentException("Category ID must be greater than zero.", nameof(id));
      }

      _logger.LogInformation("Fetching category with ID {Id}", id);
      var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

      if (category is null)
        _logger.LogWarning("⚠️ Category with ID {Id} not found", id);
      else
        _logger.LogInformation("✅ Category with ID {Id} retrieved successfully", id);

      return category;
    }
  }
}
