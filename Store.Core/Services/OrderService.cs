using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IUnitOfWork unitOfWork,
        IBasketService basketService,
        IMapper mapper,
        ILogger<OrderService> logger)
    {
      _unitOfWork = unitOfWork;
      _basketService = basketService;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<OrderToReturnDTO> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
    {
      _logger.LogInformation("Creating order for BuyerEmail: {BuyerEmail} with BasketId: {BasketId}", BuyerEmail, orderDTO.basketId);

      var basket = await _basketService.GetBasketAsync(orderDTO.basketId);
      if (!basket.Items.Any())
      {
        _logger.LogWarning("Basket {BasketId} is empty", orderDTO.basketId);
        throw new Exception("Basket is empty");
      }

      var items = new List<OrderItem>();
      decimal subtotal = 0;

      foreach (var item in basket.Items)
      {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
        if (product == null || product.Stock < item.Quantity)
        {
          _logger.LogError("Product {ProductId} not available in required quantity", item.ProductId);
          throw new Exception($"Product with ID {item.ProductId} is not available in the required quantity.");
        }

        var orderItem = new OrderItem(product.Id, item.Image!, product.Name!, product.NewPrice, item.Quantity);
        subtotal += orderItem.Price * orderItem.Quntity;
        items.Add(orderItem);

        _logger.LogInformation("Adding product {ProductName} (x{Quantity}) to order", product.Name, item.Quantity);
        await _unitOfWork.ProductRepository.UpdateAsync(product);
      }

      var deliveryMethod = await _unitOfWork.DeliveryMethodRepository.GetByIdAsync(orderDTO.deliveryMethodId);
      if (deliveryMethod == null)
      {
        _logger.LogError("Invalid delivery method {DeliveryMethodId}", orderDTO.deliveryMethodId);
        throw new Exception("Invalid delivery method.");
      }

      var shiping = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);

      var order = new Orders(BuyerEmail, subtotal, shiping, "", items, deliveryMethod);
      await _unitOfWork.OrdersRepository.AddOrederAsync(order);
      await _basketService.ClearBasketAsync(basket.Id);

      _logger.LogInformation("Order {OrderId} created successfully for BuyerEmail: {BuyerEmail}", order.Id, BuyerEmail);

      return _mapper.Map<OrderToReturnDTO>(order);
    }

    public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
    {
      _logger.LogInformation("Fetching all orders for BuyerEmail: {BuyerEmail}", BuyerEmail);
      var orders = await _unitOfWork.OrdersRepository.GetAllOrdersForUserAsync(BuyerEmail);
      return _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
    }

    public async Task<Orders?> GetOrderByIdAsync(int Id)
    {
      _logger.LogInformation("Fetching order by Id: {OrderId}", Id);
      return await _unitOfWork.OrdersRepository.GetOrderByIdAsync(Id);
    }

    public async Task AttachPaymentIntentAsync(int Id, string PaymentIntentId)
    {
      _logger.LogInformation("Attaching PaymentIntent {PaymentIntentId} to Order {OrderId}", PaymentIntentId, Id);
      var order = await GetOrderByIdAsync(Id);
      order.PaymentIntentId = PaymentIntentId;
      await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);
    }

    public async Task MarkOrderAsPaidAsync(int orderId, string paymentIntentId)
    {
      _logger.LogInformation("Marking Order {OrderId} as Paid with PaymentIntent {PaymentIntentId}", orderId, paymentIntentId);

      var order = await _unitOfWork.OrdersRepository.GetOrderByIdAsync(orderId);
      if (order == null)
      {
        _logger.LogError("Order {OrderId} not found", orderId);
        throw new Exception("Order not found");
      }

      order.status = Status.PaymentReceived;
      order.PaymentIntentId = paymentIntentId;
      order.OrderDate = DateTime.UtcNow;

      foreach (var item in order.orderItems)
      {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductItemId);
        if (product == null || product.Stock < item.Quntity)
        {
          _logger.LogError("Product {ProductId} not available for order {OrderId}", item.ProductItemId, order.Id);
          throw new Exception($"Product with ID {item.ProductItemId} is not available in the required quantity.");
        }
        product.Stock -= item.Quntity;
        await _unitOfWork.ProductRepository.UpdateAsync(product);
      }

      await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);

      _logger.LogInformation("Order {OrderId} marked as Paid successfully", order.Id);

      BackgroundJob.Enqueue<IEmailService>(emailService => emailService.SendOrderInvoiceEmailAsync(order.BuyerEmail, order));
    }

    public async Task MarkOrderAsFailedAsync(int orderId, string paymentIntentId)
    {
      _logger.LogWarning("Marking Order {OrderId} as Failed with PaymentIntent {PaymentIntentId}", orderId, paymentIntentId);

      var order = await _unitOfWork.OrdersRepository.GetOrderByIdAsync(orderId);
      if (order == null)
      {
        _logger.LogError("Order {OrderId} not found", orderId);
        throw new Exception("Order not found");
      }

      order.status = Status.PaymentFaild;
      order.PaymentIntentId = paymentIntentId;

      await _unitOfWork.OrdersRepository.UpdateOrderAsync(order);
    }

    public async Task<IReadOnlyList<DeliveryMethod>?> GetDeliveryMethodAsync()
    {
      _logger.LogInformation("Fetching delivery methods");
      return await _unitOfWork.DeliveryMethodRepository.GetDeliveryMethodsAsync();
    }

    public async Task<IEnumerable<Orders>?> GetOrdersForTodayAsync()
    {
      var today = DateTime.UtcNow.Date;
      _logger.LogInformation("Fetching all orders for today: {Today}", today);
      return await _unitOfWork.OrdersRepository.GetAllOrdersAsync(o => o.OrderDate.Date == today);
    }

    public async Task<IEnumerable<Orders>?> GetOrdersForThisMonthAsync()
    {
      var now = DateTime.UtcNow;
      var firstDay = new DateTime(now.Year, now.Month, 1);
      var lastDay = firstDay.AddMonths(1).AddDays(-1);

      _logger.LogInformation("Fetching all orders from {FirstDay} to {LastDay}", firstDay, lastDay);
      return await _unitOfWork.OrdersRepository.GetAllOrdersAsync(o => o.OrderDate.Date >= firstDay && o.OrderDate.Date <= lastDay);
    }
  }
}
