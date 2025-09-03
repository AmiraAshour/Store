using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.CategoryEntityDTO;
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

    [HttpGet]
    public  IActionResult GetCategories()
    {
      var categories =  _categoriesService.GetAllCategory();
      if (categories == null || !categories.Any())
      {
        return ApiResponseHelper.NotFound("No categories found.");
      }
      return ApiResponseHelper.Success(categories, "Categories retrieved successfully.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> getCategory(int id)
    {
      var category = await _categoriesService.GetCategoryByIdAsync(id);
      if (category == null)
      {
        return ApiResponseHelper.NotFound($"Category with ID {id} not found.");
      }
      return ApiResponseHelper.Success(category, "Category retrieved successfully.");
    }

    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<IActionResult> PostCategory(CategoryDTO categoryDto)
    {
    
      var addedCategory = await _categoriesService.AddCategoryAsync(categoryDto);

      if (addedCategory == null)
        return ApiResponseHelper.BadRequest("Failed to add category.");
      return ApiResponseHelper.Created(addedCategory, "Category created successfully.");

    }

    [Authorize(Roles ="Admin")]
    [HttpPut]
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

    [Authorize(Roles ="Admin")]
    [HttpDelete("{id}")]
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
