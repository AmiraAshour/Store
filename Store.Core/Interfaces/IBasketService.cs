using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IBasketService
  {
    Task<Basket> GetBasketAsync(string userId);
    Task<Basket> AddItemToBasketAsync(string userId, int productId, int quantity);
    Task<Basket> UpdateItemQuantityAsync(string userId, int productId, int quantity);
    Task<Basket> RemoveItemAsync(string userId, int productId);
    Task<bool> ClearBasketAsync(string userId);
  }
}
