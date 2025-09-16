using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Store.API.Controllers
{

  public class BasketsController : BaseController
  {
    private readonly IBasketService _basketService;

    public BasketsController(IBasketService basketService)
    {
      _basketService = basketService;
    }

    /// <summary>
    /// Get basket by its ID.
    /// </summary>
    [HttpGet("{BasketId}")]
    [SwaggerOperation(
      Summary = "Retrieve basket",
      Description = "Returns the basket with all items by basket ID."
    )]
    public async Task<IActionResult> GetBasket(string BasketId)
    {
      var basket = await _basketService.GetBasketAsync(BasketId);
      return ApiResponseHelper.Success(basket, "Basket retrieved successfully");
    }

    /// <summary>
    /// Add a new item to the basket.
    /// </summary>
    [HttpPost("add-item/{BasketId}")]
    [SwaggerOperation(
      Summary = "Add item to basket",
      Description = "Adds a new item to the basket or updates it if already exists."
    )]
    public async Task<IActionResult> AddItem(string BasketId, [FromBody] BasketItemDTO basketDto)
    {
      var basket = await _basketService.AddItemToBasketAsync(BasketId, basketDto.ProductId, basketDto.Quantity);
      return ApiResponseHelper.Created(basket, "Item added successfully");
    }

    /// <summary>
    /// Update quantity of an item in the basket.
    /// </summary>
    [HttpPut("update-item/{BasketId}")]
    [SwaggerOperation(
      Summary = "Update basket item",
      Description = "Updates the quantity of a product in the basket. If quantity = 0 the item is removed."
    )]
    public async Task<IActionResult> UpdateItem(string BasketId, [FromBody] BasketItemDTO basket)
    {
      var updatedBasket = await _basketService.UpdateItemQuantityAsync(BasketId, basket.ProductId, basket.Quantity);
      return ApiResponseHelper.Created(updatedBasket, "Item updated successfully");
    }

    /// <summary>
    /// Remove an item from the basket by product ID.
    /// </summary>
    [HttpDelete("delete-item/{BasketId}/{productId}")]
    [SwaggerOperation(
      Summary = "Delete item from basket",
      Description = "Removes a specific product from the basket."
    )]
    public async Task<IActionResult> RemoveItem(string BasketId, int productId)
    {
      var basket = await _basketService.RemoveItemAsync(BasketId, productId);
      return ApiResponseHelper.Success(basket, "Item deleted successfully");
    }

    /// <summary>
    /// Clear basket (remove all items).
    /// </summary>
    [HttpDelete("clear-basket/{BasketId}")]
    [SwaggerOperation(
      Summary = "Clear basket",
      Description = "Deletes the entire basket with all items."
    )]
    public async Task<IActionResult> ClearBasket(string BasketId)
    {
      var success = await _basketService.ClearBasketAsync(BasketId);
      if (!success)
        return ApiResponseHelper.NotFound("Basket not found");

      return ApiResponseHelper.Success("", "Basket deleted successfully");
    }
  }
}
