using AutoMapper;
using Store.Core.DTO.Order;
using Store.Core.Entities.Order;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class OrderService : IOrderService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketService _basketService;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IBasketService basketService, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _basketService = basketService;
      _mapper = mapper;
    }

    public async Task<OrderToReturnDTO> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
    {
      var basket = await _basketService.GetBasketAsync(orderDTO.basketId);
      var items = new List<OrderItem>();

      decimal subtotal = 0;
      foreach (var item in basket.Items)
      {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
        if (product == null || product.Stock < item.Quantity)
          throw new Exception($"Product with ID {item.ProductId} is not available in the required quantity.");
        var orderItem = new OrderItem(product.Id, item.Image!, product.Name!, product.NewPrice, item.Quantity);
        subtotal += orderItem.Price * orderItem.Quntity;
        items.Add(orderItem);
        product.Stock -= item.Quantity;
        await _unitOfWork.ProductRepository.UpdateAsync(product);

      }
      var deliveryMethod = await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(orderDTO.deliveryMethodId);
      if (deliveryMethod == null)
        throw new Exception("Invalid delivery method.");

      var shiping = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);
      var order = new Orders(BuyerEmail, subtotal, shiping, "", items, deliveryMethod);

      await _unitOfWork.OrdersRepository.AddOrederAsync(order);
      await _basketService.ClearBasketAsync(basket.Id);
      var orderToReturn = _mapper.Map<OrderToReturnDTO>(order);
      return orderToReturn;
    }

    public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
    {
      var orders=await _unitOfWork.OrdersRepository.GetAllOrdersForUserAsync(BuyerEmail);
      var ordersToReturn= _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
      return ordersToReturn;
    }


    public async Task<OrderToReturnDTO?> GetOrderByIdAsync(int Id, string BuyerEmail)
    {
      var order=await _unitOfWork.OrdersRepository.GetOrderByIdAsync(Id, BuyerEmail);
      var orderToReturn= _mapper.Map<OrderToReturnDTO>(order);
      return orderToReturn;
    }
    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync()
    {
     return await _unitOfWork.DeliveryMethodRepository.GetDeliveryMethodsAsync();
    }
  }
}
