namespace Store.Core.Entities.comman
{
  public class PaymentIntentCreateResponse
  {
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
  }
}
