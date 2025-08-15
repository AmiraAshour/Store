using Store.Core.DTO;
using Store.Core.Entities.Product;
using Store.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IProductsService
  {

    /// <summary>
    /// Asynchronously retrieves all products.
    /// </summary>
    /// <remarks>
    /// The returned collection may be empty if no products are available. This method does not
    /// filter  or paginate the results; all products are included in the response.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of Product objects representing all available products.
    /// </returns>
    Task<IEnumerable<ProductDTO>?> GetAllProductsAsync(ProductParams param);


    /// <summary>
    /// Retrieves a product by its unique identifier asynchronously.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch a product from the data source.
    /// Ensure that the <paramref name="id"/> parameter is valid to avoid exceptions.</remarks>
    /// <param name="id">The unique identifier of the product to retrieve. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Product"/> object
    /// corresponding to the specified identifier, or <see langword="null"/> if no product is found.</returns>
    Task<ProductDTO?> GetProductByIdAsync(int id);
    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <returns>The added product.</returns>
    Task<ProductDTO?> AddProductAsync(AddProductDTO? product);
   
    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    Task<bool> DeleteProductAsync(int id);
   

    /// <summary>
    /// Updates the specified product in the system.
    /// </summary>
    /// <remarks>Use this method to update an existing product's details in the system. Ensure that the
    /// product object contains the necessary updates and a valid identifier before calling this method.</remarks>
    /// <param name="product">The product to update. The product must have a valid identifier and contain the updated details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated product with the latest
    /// details as stored in the system.</returns>
    Task<ProductDTO?> UpdateProductAsync(UpdateProductDTO? product);
  }
}
