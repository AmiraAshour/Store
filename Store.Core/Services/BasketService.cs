using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;
using System.Text.Json;

namespace Store.infrastructure.Repositories
{
  public class BasketService : IBasketService
  {
    private readonly IUnitOfWork _unitOfWork;

    public BasketService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public async Task<Basket> GetBasketAsync(string userId)
    {
      return await _unitOfWork.BasketRepository.GetBasketAsync(userId) ?? new Basket(userId);
    }

    public async Task<Basket> AddItemToBasketAsync(string userId, int productId, int quantity)
    {
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, p => p.Category, p => p.Photos);
      if (product == null) throw new Exception("Product not found");
      if (product.Stock < quantity) throw new Exception("Not enough stock");

      var basket = await GetBasketAsync(userId);
      var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (existingItem != null)
      {
        existingItem.Quantity = quantity;
      }
      else
      {
        basket.Items.Add(new BasketItem
        {
          ProductId = product.Id,
          ProductName = product.Name,
          Price = product.NewPrice,
          Quantity = quantity,
          CategoryName = product.Category?.Name,
          Image = product.Photos?.FirstOrDefault()?.ImageName,
        });
      }

      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      return basket;
    }

    public async Task<Basket> UpdateItemQuantityAsync(string userId, int productId, int quantity)
    {
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
      if (product == null) throw new Exception("Product not found");
      if (product.Stock < quantity) throw new Exception("Not enough stock of Product");

      var basket = await GetBasketAsync(userId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
      if (item == null) throw new Exception("Item not found");

      if (quantity <= 0)
        basket.Items.Remove(item);
      else
        item.Quantity = quantity;

      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      return basket;
    }

    public async Task<Basket> RemoveItemAsync(string userId, int productId)
    {
      var basket = await GetBasketAsync(userId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
      if (item == null) return basket;

      basket.Items.Remove(item);
      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      return basket;
    }

    public async Task<bool> ClearBasketAsync(string userId)
    {
      return await _unitOfWork.BasketRepository.DeleteBasketAsync(userId);
    }
  }
}
