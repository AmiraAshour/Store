using Store.Core.Entities;
using Store.Core.Interfaces;

namespace Store.infrastructure.Repositories
{
  public class BasketService : IBasketService
  {
    private readonly IUnitOfWork _unitOfWork;

    public BasketService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public async Task<Basket> GetBasketAsync(string BasketId)
    {
      return await _unitOfWork.BasketRepository.GetBasketAsync(BasketId) ?? new Basket(BasketId);
    }

    public async Task<Basket> AddItemToBasketAsync(string BasketId, int productId, int quantity)
    {
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, p => p.Category, p => p.Photos);

      if (product == null) throw new Exception("Product not found");

      if (product.Stock < quantity) throw new Exception("Not enough stock");

      var basket = await GetBasketAsync(BasketId);
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

    public async Task<Basket> UpdateItemQuantityAsync(string BasketId, int productId, int quantity)
    {
      var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

      if (product == null) throw new Exception("Product not found");
      
      if (product.Stock < quantity) throw new Exception("Not enough stock of Product");

      var basket = await GetBasketAsync(BasketId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (item == null) throw new Exception("Item not found");

      if (quantity <= 0)
        basket.Items.Remove(item);
      else
        item.Quantity = quantity;

      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      return basket;
    }

    public async Task<Basket> RemoveItemAsync(string BasketId, int productId)
    {
      var basket = await GetBasketAsync(BasketId);
      var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);

      if (item == null)
        throw new Exception("Item not found");

      basket.Items.Remove(item);
      await _unitOfWork.BasketRepository.UpdateBasketAsync(basket);
      return basket;
    }

    public async Task<bool> ClearBasketAsync(string BasketId)
    {
      return await _unitOfWork.BasketRepository.DeleteBasketAsync(BasketId);
    }
  }
}
