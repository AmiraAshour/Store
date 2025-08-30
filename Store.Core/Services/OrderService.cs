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
      if (!basket.Items.Any())
        throw new Exception("Basket is empty");

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
      var orders = await _unitOfWork.OrdersRepository.GetAllOrdersForUserAsync(BuyerEmail);
      var ordersToReturn = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
      return ordersToReturn;
    }


    public async Task<Orders?> GetOrderByIdAsync(int Id)
    {
      var order = await _unitOfWork.OrdersRepository.GetOrderByIdAsync(Id);
      return order;
    }
    public async Task AttachPaymentIntentAsync(int Id, string PaymentIntentId)
    {
      var order = await GetOrderByIdAsync(Id);
      order.PaymentIntentId = PaymentIntentId;
      await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);

    }
    public async Task MarkOrderAsPaidAsync(int orderId, string paymentIntentId)
    {
      var order = await _unitOfWork.OrdersRepository.GetOrderByIdAsync(orderId);
      if (order == null)
        throw new Exception("Order not found");

      order.status = Status.PaymentReceived;
      order.PaymentIntentId = paymentIntentId;
      order.OrderDate = DateTime.UtcNow;
      foreach (var item in order.orderItems)
      {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductItemId);
        if (product == null || product.Stock < item.Quntity)
          throw new Exception($"Product with ID {item.ProductItemId} is not available in the required quantity.");
        product.Stock -= item.Quntity;
        await _unitOfWork.ProductRepository.UpdateAsync(product);
      }

      await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);

    }

    public async Task MarkOrderAsFailedAsync(int orderId, string paymentIntentId)
    {
      var order = await _unitOfWork.OrdersRepository.GetOrderByIdAsync(orderId);
      if (order == null)
        throw new Exception("Order not found");

      order.status = Status.PaymentFaild;
      order.PaymentIntentId = paymentIntentId;

     await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);
    }
    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync()
    {
      return await _unitOfWork.DeliveryMethodRepository.GetDeliveryMethodsAsync();
    }

  }
}
