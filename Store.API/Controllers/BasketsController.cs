using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Core.DTO.Order;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.infrastructure.Repositories;

namespace Store.API.Controllers
{

  public class BasketsController : BaseController
  {
    private readonly IBasketService _basketService;

    public BasketsController(IBasketService basketService)
    {
      _basketService = basketService;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetBasket(string Id)
    {
      var basket = await _basketService.GetBasketAsync(Id);
   
    return ApiResponseHelper.Success(basket, "Basket retrieved successfully");
    }

    [HttpPost("{Id}")]
    public async Task<IActionResult> AddItem(string Id, [FromBody] BasketItemDTO basketDto)
    {
      var basket = await _basketService.AddItemToBasketAsync(Id, basketDto.ProductId, basketDto.Quantity);
      return ApiResponseHelper.Created(basket, "Item added successfully");

    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateItem(string Id,BasketItemDTO basket)
    {
      var updatedBasket = await _basketService.UpdateItemQuantityAsync(Id,basket.ProductId,basket.Quantity);
     
      return ApiResponseHelper.Created(updatedBasket, "Item updated successfully");
    }


    [HttpDelete("{Id}/{productId}")]
    public async Task<IActionResult> RemoveItem(string Id, int productId)
    {
      var basket = await _basketService.RemoveItemAsync(Id, productId);
     
      return ApiResponseHelper.Success(basket, "Item deleted successfully");
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> ClearBasket(string Id)
    {
      var success = await _basketService.ClearBasketAsync(Id);
      if (!success)
      {
        return ApiResponseHelper.NotFound("Basket not found");
      }
      return ApiResponseHelper.Success("", "Basket deleted successfully");
    }
   
  }
}
