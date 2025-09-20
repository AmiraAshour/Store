using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Store.API.Helper;
using Store.Core.DTO.CategoryEntityDTO;
using Store.Core.Entities.ProductEntity;
using Store.Core.Interfaces.ServiceInterfaces;

namespace Store.API.Controllers
{
  public class CategoriesController : BaseController
  {
    private readonly ICategoriesService _categoriesService;
    public CategoriesController(ICategoriesService categoriesService)
    {
      _categoriesService = categoriesService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all categories", Description = "Returns a list of all available categories.")]
    [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetCategories()
    {
      var categories = _categoriesService.GetAllCategory();
      if (categories == null || !categories.Any())
      {
        return ApiResponseHelper.NotFound("No categories found.");
      }
      return ApiResponseHelper.Success(categories, "Categories retrieved successfully.");
    }

    /// <summary>
    /// Get a category by ID
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieve category by ID", Description = "Fetches a category by its unique identifier.")]
    [ProducesResponseType(typeof(Category), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> getCategory(int id)
    {
      var category = await _categoriesService.GetCategoryByIdAsync(id);
      if (category == null)
      {
        return ApiResponseHelper.NotFound($"Category with ID {id} not found.");
      }
      return ApiResponseHelper.Success(category, "Category retrieved successfully.");
    }

    /// <summary>
    /// Add a new category
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new category", Description = "Adds a new category to the system (Admin only).")]
    [ProducesResponseType(typeof(Category), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostCategory(CategoryDTO categoryDto)
    {
      var addedCategory = await _categoriesService.AddCategoryAsync(categoryDto);

      if (addedCategory == null)
        return ApiResponseHelper.BadRequest("Failed to add category.");
      return ApiResponseHelper.Created(addedCategory, "Category created successfully.");
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update category", Description = "Updates the details of an existing category (Admin only).")]
    [ProducesResponseType(typeof(Category), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutCategory(UpdateCategoryDTO categoryDto)
    {
      if (categoryDto == null || categoryDto.Id <= 0)
      {
        return ApiResponseHelper.BadRequest("Valid category data is required.");
      }

      var result = await _categoriesService.UpdateCategoryAsync(categoryDto);

      if (result is null)
      {
        return ApiResponseHelper.NotFound($"Category with ID {categoryDto.Id} not found.");
      }

      return ApiResponseHelper.Success(result, "Category has been updated successfully.");
    }

    /// <summary>
    /// Delete a category by ID
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete category", Description = "Deletes an existing category by its ID (Admin only).")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> deleteCategory(int id)
    {
      if (id <= 0)
      {
        return ApiResponseHelper.BadRequest("Valid category ID is required.");
      }

      var result = await _categoriesService.DeleteCategoryAsync(id);
      if (!result)
      {
        return ApiResponseHelper.NotFound($"Category with ID {id} not found.");
      }

      return ApiResponseHelper.Success<string>(null, "Category has been deleted successfully.");
    }
  }
}
