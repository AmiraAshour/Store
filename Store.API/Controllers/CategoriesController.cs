using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
  public class CategoriesController : BaseController
  {
    private readonly ICategoriesService _categoriesService;
    public CategoriesController(ICategoriesService categoriesService) 
    {
      _categoriesService = categoriesService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> get()
    {
      var categories = await _categoriesService.GetAllCategoryAsync();
      if (categories == null || !categories.Any())
      {
        return ApiResponseHelper.NotFound("No categories found.");
      }
      return ApiResponseHelper.Success(categories, "Categories retrieved successfully.");
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> getById(int id)
    {
      var category = await _categoriesService.GetCategoryByIdAsync(id);
      if (category == null)
      {
        return ApiResponseHelper.NotFound($"Category with ID {id} not found.");
      }
      return ApiResponseHelper.Success(category, "Category retrieved successfully.");
    }

    [HttpPost("add-category")]
    public async Task<IActionResult> addCategory(CategoryDTO categoryDto)
    {
    
      var addedCategory = await _categoriesService.AddCategoryAsync(categoryDto);

      if (addedCategory == null)
        return ApiResponseHelper.BadRequest("Failed to add category.");
      return ApiResponseHelper.Created(addedCategory, "Category created successfully.");

    }

    [HttpPut("update-category")]
    public async Task<IActionResult> updateCategory(UpdateCategoryDTO categoryDto)
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

    [HttpDelete("delete-category/{id}")]
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
