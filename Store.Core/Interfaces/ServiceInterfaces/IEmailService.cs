using Store.Core.Entities.Order;

namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IEmailService
  {
    Task SendConfirmationEmailAsync(string email, string userId, string token, string clientUrl);
    Task SendResetPasswordEmailAsync(string email, string userId, string token, string clientUrl);

    Task SendOrderInvoiceEmailAsync(string to, Orders order);
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName);
  }

 

}
