using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO;
using Store.Core.Interfaces;


namespace Store.API.Controllers
{

  public class ProductController : BaseController
  {
    private readonly IProductsService _productService;

    public ProductController(IProductsService productService)
    {
      _productService = productService;
    }

    // GET: api/<ProductController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var products= await _productService.GetAllProductsAsync();
      if (products == null || !products.Any())
      {
        return ApiResponseHelper.NotFound("No Product found.");
      }
      return ApiResponseHelper.Success(products, "Categories retrieved successfully.");
    }

    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var product =await _productService.GetProductByIdAsync(id);
      if (product == null)
      {
        return ApiResponseHelper.NotFound($"Product with ID {id} not found.");
      }
      return ApiResponseHelper.Success(product, "Product retrieved successfully.");
    }

    // POST api/<ProductController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProductDTO productDTO)
    {
      var result =await _productService.AddProductAsync(productDTO);
      if (result == null)
      {
        return ApiResponseHelper.BadRequest("Failed to add product.");
      }
      return ApiResponseHelper.Created(result, "Product created successfully.");
    }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(UpdateProductDTO productDTO)
    {
      if (productDTO == null || productDTO.Id <= 0)
      {
        return ApiResponseHelper.BadRequest("Valid product data is required.");
      }
      var result =await _productService.UpdateProductAsync(productDTO);
      if (result == null)
      {
        return ApiResponseHelper.BadRequest("Failed to update product.");
      }
       return ApiResponseHelper.Success(result, "Product updated successfully.");
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      if (id <= 0)
      {
       return ApiResponseHelper.BadRequest("Product ID must be greater than zero.");
      }
      var result =await _productService.DeleteProductAsync(id);
      if (!result)
      {
       return ApiResponseHelper.NotFound($"Product with ID {id} not found.");
      }
    return  ApiResponseHelper.Success( "","Product deleted successfully.");
    }
  }
}
