using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Product;
using Store.Core.Interfaces;
using Store.Core.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace Store.API.Controllers
{
  public class ProductController : BaseController
  {
    private readonly IProductsService _productService;

    public ProductController(IProductsService productService)
    {
      _productService = productService;
    }

    /// <summary>
    /// Get all products with optional filtering, searching, and pagination.
    /// </summary>
    /// <remarks>
    /// You can filter by:
    /// - Search text (name, description)
    /// - Category ID
    ///  
    /// You can also apply:
    /// - Sorting (ASC/DESC by price)
    /// - Pagination (page number, page size)
    /// </remarks>
    /// <param name="param">Filter and pagination parameters</param>
    /// <response code="200">Returns a paginated list of products</response>
    /// <response code="404">If no products are found</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all products", Description = "Retrieves a list of products with filtering, search, and pagination.")]
    public async Task<IActionResult> GetProducts([FromQuery] ProductParams param)
    {
      var products = await _productService.GetAllProductsAsync(param);
      if (products == null || !products.Any())
      {
        return ApiResponseHelper.NotFound("No Product found.");
      }
      return ApiResponseHelper.Success(
        new Pagination<ProductDTO>(param.PageNumber, param.pageSize, param.pageSize, products.ToList()),
        "Products retrieved successfully."
      );
    }

    /// <summary>
    /// Get a single product by ID.
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <response code="200">Returns the product</response>
    /// <response code="404">If product not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get product by ID", Description = "Retrieves details of a single product.")]
    public async Task<IActionResult> GetProduct(int id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      if (product == null)
      {
        return ApiResponseHelper.NotFound($"Product with ID {id} not found.");
      }
      return ApiResponseHelper.Success(product, "Product retrieved successfully.");
    }

    /// <summary>
    /// Create a new product (Admin only).
    /// </summary>
    /// <param name="productDTO">The product details</param>
    /// <response code="201">If product created successfully</response>
    /// <response code="400">If creation failed</response>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create product", Description = "Creates a new product. Requires Admin role.")]
    public async Task<IActionResult> PostProduct(AddProductDTO productDTO)
    {
      var result = await _productService.AddProductAsync(productDTO);
      if (result == null)
      {
        return ApiResponseHelper.BadRequest("Failed to add product.");
      }
      return ApiResponseHelper.Created(result, "Product created successfully.");
    }

    /// <summary>
    /// Update an existing product (Admin only).
    /// </summary>
    /// <param name="productDTO">The updated product details</param>
    /// <response code="200">If product updated successfully</response>
    /// <response code="400">If update failed or invalid data</response>
    [Authorize(Roles = "Admin")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update product", Description = "Updates an existing product. Requires Admin role.")]
    public async Task<IActionResult> PutProduct(UpdateProductDTO productDTO)
    {
      if (productDTO == null || productDTO.Id <= 0)
      {
        return ApiResponseHelper.BadRequest("Valid product data is required.");
      }
      var result = await _productService.UpdateProductAsync(productDTO);
      if (result == null)
      {
        return ApiResponseHelper.BadRequest("Failed to update product.");
      }
      return ApiResponseHelper.Success(result, "Product updated successfully.");
    }

    /// <summary>
    /// Delete a product by ID (Admin only).
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <response code="200">If product deleted successfully</response>
    /// <response code="400">If ID is invalid</response>
    /// <response code="404">If product not found</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete product", Description = "Deletes a product by ID. Requires Admin role.")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      if (id <= 0)
      {
        return ApiResponseHelper.BadRequest("Product ID must be greater than zero.");
      }
      var result = await _productService.DeleteProductAsync(id);
      if (!result)
      {
        return ApiResponseHelper.NotFound($"Product with ID {id} not found.");
      }
      return ApiResponseHelper.Success("", "Product deleted successfully.");
    }
  }
}
