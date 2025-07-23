using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.DTO;
using Store.Core.Entities.Product;
using Store.Core.Interfaces;

namespace Store.API.Controllers
{
  public class CategoriesController : BaseController
  {
    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> get()
    {
        var category = await Work.CategoryRepository.GetAllAsync();
        if (category == null || !category.Any())
        {
          return NotFound("No categories found.");
        }
        return Ok(category);

    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> getById(int id)
    {
      var category = await Work.CategoryRepository.GetByIdAsync(id);
      if (category == null)
      {
        return NotFound($"Category with ID {id} not found.");
      }
      return Ok(category);
    }
    [HttpPost("add-category")]
    public async Task<IActionResult> addCategory( CategoryDTO categoryDto)
    {
      if (categoryDto == null)
      {
        return BadRequest("Category data is required.");
      }
      var category = mapper.Map<Category>(categoryDto);

       await Work.CategoryRepository.AddAsync(category);
    
      return Ok(new { Message="Item has been added", category });
    }

    [HttpPut("update-category")]
    public async Task<IActionResult> updateCategory(UpdateCategoryDTO categoryDto)
    {
      if (categoryDto == null || categoryDto.Id <= 0)
      {
        return BadRequest("Valid category data is required.");
      }
      var category = mapper.Map<Category>(categoryDto);

      var resulte= await Work.CategoryRepository.UpdateAsync(category);
      if (!resulte)
      {
        return NotFound($"Category with ID {categoryDto.Id} not found.");
      }
      
      
      return Ok(new { Message="Item has been updated", category });
    }

    [HttpDelete("delete-category/{id}")]
    public async Task<IActionResult> deleteCategory(int id)
    {
      if (id <= 0)
      {
        return BadRequest("Valid category ID is required.");
      }
     var result= await Work.CategoryRepository.DeleteAsync(id);
      if(!result)
      {
        return NotFound($"Category with ID {id} not found.");
      }
      return Ok(new { Message = "Item has been deleted" });
    }
  }
}
