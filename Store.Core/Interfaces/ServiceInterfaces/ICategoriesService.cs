using Store.Core.DTO.CategoryEntityDTO;
using Store.Core.Entities.ProductEntity;
namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface ICategoriesService
  {

    /// <summary>
    /// Added category details  
    /// </summary>
    /// <param name="categoryDto"> Category details to Added </param>
    /// <returns>Returns category object after Added or null</returns>

    Task<Category?> AddCategoryAsync(CategoryDTO categoryDto);

    /// <summary>
    /// Deleted the specified category based on the given category ID   
    /// </summary>
    /// <param name="id"> Category id</param>
    /// <returns>Returns true, if the deletion is successful; otherwise false</returns>

    Task<bool> DeleteCategoryAsync(int id);


    /// <summary>
    /// retrieves all categories from the database  
    /// </summary>
    /// <returns>Returns all categories </returns>

    IEnumerable<Category> GetAllCategory();


    /// <summary>
    /// return the specified category based on the given category ID   
    /// </summary>
    /// <param name="id"> Category id</param>
    /// <returns>Returns the specified category  object </returns>

    Task<Category?> GetCategoryByIdAsync(int id);


    /// <summary>
    /// Updates the specified category details based on the given category ID   
    /// </summary>
    /// <param name="categoryDto"> Category details to update, including Category id</param>
    /// <returns>Returns the Category object after updation or null </returns>

    Task<Category?> UpdateCategoryAsync(UpdateCategoryDTO categoryDto);
  }
}
