using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IDeliveryMethodRepository
  {
    Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodsAsync();
    Task<DeliveryMethod?> GetByIdAsync(int id);
  }
}
