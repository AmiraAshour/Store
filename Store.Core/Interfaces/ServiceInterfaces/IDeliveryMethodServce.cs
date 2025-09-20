using Store.Core.DTO.Order;
using Store.Core.Entities.Order;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IDeliveryMethodServce
  {
    Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync();
    Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(int id);
    Task<DeliveryMethod> AddDeliveryMethodAsync(DeliveryMethodDTO method);
    Task<DeliveryMethod?> UpdateDeliveryMethodAsync(int id,DeliveryMethodDTO method);
    Task<bool> DeleteDeliveryMethodAsync(int id);

  }
}
