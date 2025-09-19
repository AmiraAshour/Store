using Store.Core.Entities;
using Store.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Store.infrastructure.Repositories
{
  public class BasketService : IBasketService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BasketService> _logger;

    public BasketService(IUnitOfWork unitOfWork, ILogger<BasketService> logger)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public async Task<Basket> GetBasketAsync(string BasketId)
    {
      _logger.LogInformation("Retrieving basket with ID: {BasketId}", BasketId);
      var basket = await _unitOfWork.BasketRepository.GetBasketAsync(BasketId) ?? new Basket(BasketId);
      _logger.LogInformation("Basket retrieved with {ItemCount} items.", basket.Items.Count);
      return basket;
    }

    public async Task<Basket> AddItemToBasketAsync(string BasketId, int productId, int quantity)
    {
      _logger.LogInformation("Adding product {ProductId} (qty: {Quantity}) to basket {BasketId}", productId, quantity, BasketId);
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, p => p.Category, p => p.Photos);

      if (product == null)
      {
        _logger.LogWarning("Product {ProductId} not found when adding to basket {BasketId}", productId, BasketId);
        throw new Exception("Product not found");
      }

      if (product.Stock < quantity)
      {
        _logger.LogWarning("Not enough stock for product {ProductId} (requested: {Quantity}, available: {Stock})", productId, quantity, product.Stock);
        throw new Exception("Not enough stock");
      }

      var basket = await GetBasketAsync(BasketId);
      var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (existingItem != null)
      {
        _logger.LogInformation("Updating quantity for product {ProductId} in basket {BasketId}", productId, BasketId);
        existingItem.Quantity = quantity;
      }
      else
      {
        _logger.LogInformation("Adding new product {ProductId} to basket {BasketId}", productId, BasketId);
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
      _logger.LogInformation("Basket {BasketId} updated. Total items: {ItemCount}", BasketId, basket.Items.Count);
      return basket;
    }

    public async Task<Basket> UpdateItemQuantityAsync(string BasketId, int productId, int quantity)
    {
      _logger.LogInformation("Updating quantity for product {ProductId} in basket {BasketId} to {Quantity}", productId, BasketId, quantity);
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

      if (product == null)
      {
        _logger.LogWarning("Product {ProductId} not found when updating quantity in basket {BasketId}", productId, BasketId);
        throw new Exception("Product not found");
      }
      
      if (product.Stock < quantity)
      {
        _logger.LogWarning("Not enough stock for product {ProductId} (requested: {Quantity}, available: {Stock})", productId, quantity, product.Stock);
        throw new Exception("Not enough stock of Product");
      }

      var basket = await GetBasketAsync(BasketId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (item == null)
      {
        _logger.LogWarning("Item for product {ProductId} not found in basket {BasketId}", productId, BasketId);
        throw new Exception("Item not found");
      }

      if (quantity <= 0)
      {
        _logger.LogInformation("Removing product {ProductId} from basket {BasketId} due to zero quantity", productId, BasketId);
        basket.Items.Remove(item);
      }
      else
      {
        _logger.LogInformation("Setting quantity for product {ProductId} in basket {BasketId} to {Quantity}", productId, BasketId, quantity);
        item.Quantity = quantity;
      }

      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      _logger.LogInformation("Basket {BasketId} updated after quantity change. Total items: {ItemCount}", BasketId, basket.Items.Count);
      return basket;
    }

    public async Task<Basket> RemoveItemAsync(string BasketId, int productId)
    {
      _logger.LogInformation("Removing product {ProductId} from basket {BasketId}", productId, BasketId);
      var basket = await GetBasketAsync(BasketId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (item == null)
      {
        _logger.LogWarning("Item for product {ProductId} not found in basket {BasketId} when removing", productId, BasketId);
        throw new Exception("Item not found");
      }

      basket.Items.Remove(item);
      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      _logger.LogInformation("Product {ProductId} removed from basket {BasketId}. Total items: {ItemCount}", productId, BasketId, basket.Items.Count);
      return basket;
    }

    public async Task<bool> ClearBasketAsync(string BasketId)
    {
      _logger.LogInformation("Clearing basket {BasketId}", BasketId);
      var result = await _unitOfWork.BasketRepository.DeleteBasketAsync(BasketId);
      _logger.LogInformation("Basket {BasketId} cleared: {Result}", BasketId, result);
      return result;
    }
  }
}
