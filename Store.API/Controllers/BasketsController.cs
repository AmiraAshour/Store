using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Interfaces;

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
    /// Get the basket by id
    /// </summary>
    /// <param name="BasketId"> basket id</param>
    /// <returns> basket items </returns>

    [HttpGet("{BasketId}")]
    public async Task<IActionResult> GetBasket(string BasketId)
    {
      var basket = await _basketService.GetBasketAsync(BasketId);
   
    return ApiResponseHelper.Success(basket, "Basket retrieved successfully");
    }

    [HttpPost("add-item/{BasketId}")]
    public async Task<IActionResult> AddItem(string BasketId, [FromBody] BasketItemDTO basketDto)
    {
      var basket = await _basketService.AddItemToBasketAsync(BasketId, basketDto.ProductId, basketDto.Quantity);
      return ApiResponseHelper.Created(basket, "Item added successfully");

    }

    [HttpPut("update-item/{BasketId}")]
    public async Task<IActionResult> UpdateItem(string BasketId,BasketItemDTO basket)
    {
      var updatedBasket = await _basketService.UpdateItemQuantityAsync(BasketId,basket.ProductId,basket.Quantity);
     
      return ApiResponseHelper.Created(updatedBasket, "Item updated successfully");
    }


    [HttpDelete("delete-item/{BasketId}/{productId}")]
    public async Task<IActionResult> RemoveItem(string BasketId, int productId)
    {
      var basket = await _basketService.RemoveItemAsync(BasketId, productId);
     
      return ApiResponseHelper.Success(basket, "Item deleted successfully");
    }

    [HttpDelete("clear-basket/{BasketId}")]
    public async Task<IActionResult> ClearBasket(string BasketId)
    {
      var success = await _basketService.ClearBasketAsync(BasketId);
      if (!success)
      {
        return ApiResponseHelper.NotFound("Basket not found");
      }
      return ApiResponseHelper.Success("", "Basket deleted successfully");
    }
   
  }
}
