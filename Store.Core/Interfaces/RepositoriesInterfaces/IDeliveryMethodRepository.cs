using Store.Core.Entities.Order;

namespace Store.Core.Interfaces.RepositoriesInterfaces
{
  public interface IDeliveryMethodRepository
  {
    Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodsAsync();
    Task<DeliveryMethod?> GetByIdAsync(int id);
    Task  AddAsync(DeliveryMethod method);
    Task  UpdateAsync(DeliveryMethod method);
    Task DeleteAsync(DeliveryMethod method);
  }
}
