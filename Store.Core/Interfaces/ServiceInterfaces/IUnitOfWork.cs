using Store.Core.Interfaces.RepositoriesInterfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IUnitOfWork
  {
    public ICategoryRepository CategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IPhotoRepository PhotoRepository { get; }
    public IBasketRepository BasketRepository { get; }
    public IDeliveryMethodRepository DeliveryMethodRepository  { get; }
    public IOrdersRepository OrdersRepository { get; }
    public IAddressRepository AddressRepository { get; }
    public IReviewRepository ReviewRepository { get; }
    public IWishlistRepository wishlistRepository { get; }
  }
}
