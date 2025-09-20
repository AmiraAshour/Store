using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.BasketEntity
{
  public class Basket
  {
    public Basket() { }
    public Basket(string id)
    {
      Id = id;
    }
    public string Id { get; set; } = "";
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

  }
}
