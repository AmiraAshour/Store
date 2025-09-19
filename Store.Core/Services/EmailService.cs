using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Store.Core.Entities.EntitySettings;
using Store.Core.Entities.Order;
using Store.Core.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Store.Core.Services
{
  public class EmailService : IEmailService
  {
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
      _emailSettings = emailSettings.Value;
      _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
      try
      {
        using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
          client.Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
          client.EnableSsl = true;

          var mailMessage = new MailMessage
          {
            From = new MailAddress(_emailSettings.From, _emailSettings.Name),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
          };

          mailMessage.To.Add(to);

          _logger.LogInformation("📧 Sending email to {To} with subject '{Subject}'", to, subject);
          await client.SendMailAsync(mailMessage);
          _logger.LogInformation("✅ Email to {To} sent successfully", to);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "❌ Failed to send email to {To}", to);
        throw;
      }
    }

    public async Task SendConfirmationEmailAsync(string email, string userId, string token, string clientUrl)
    {
      _logger.LogInformation("Preparing confirmation email for {Email}", email);
      string confirmUrl = $"{clientUrl}?userId={userId}&token={Uri.EscapeDataString(token)}";

      string body = $@"
        <html>
        <body>
          <h2>Welcome to MyApp 🎉</h2>
          <p>Please confirm your email:</p>
          <a href=""{confirmUrl}"">Confirm Email</a>
        </body>
        </html>";

      await SendEmailAsync(email, "Confirm your email", body);
    }

    public async Task SendResetPasswordEmailAsync(string email, string userId, string token, string clientUrl)
    {
      _logger.LogInformation("Preparing reset password email for {Email}", email);
      string resetUrl = $"{clientUrl}?userId={userId}&token={Uri.EscapeDataString(token)}";

      string body = $@"
        <html>
        <body>
          <h2>Password Reset Request 🔑</h2>
          <p>Click below to reset:</p>
          <a href=""{resetUrl}"">Reset Password</a>
        </body>
        </html>";

      await SendEmailAsync(email, "Reset your password", body);
    }

    public async Task SendOrderInvoiceEmailAsync(string to, Orders order)
    {
      _logger.LogInformation("Preparing invoice email for Order #{OrderId} to {Email}", order.Id, to);

      string body = $@"
        <html>
        <body>
          <h2>Invoice for Order #{order.Id}</h2>
          <p>Date: {order.OrderDate:dd MMM yyyy}</p>
          <p>Status: {order.status}</p>
          <hr/>
          <ul>
            {string.Join("", order.orderItems.Select(i => $"<li>{i.ProductName} - {i.Quntity} x {i.Price:C}</li>"))}
          </ul>
          <p><b>Total: {order.GetTotal():C}</b></p>
        </body>
        </html>";

      await SendEmailAsync(to, $"Invoice for Order #{order.Id}", body);
    }

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
    {
      try
      {
        using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
          client.Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
          client.EnableSsl = true;

          var mailMessage = new MailMessage
          {
            From = new MailAddress(_emailSettings.From, _emailSettings.Name),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
          };

          mailMessage.To.Add(to);
          mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), attachmentName));

          _logger.LogInformation("📎 Sending email with attachment '{Attachment}' to {To}", attachmentName, to);
          await client.SendMailAsync(mailMessage);
          _logger.LogInformation("✅ Email with attachment sent to {To}", to);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "❌ Failed to send email with attachment to {To}", to);
        throw;
      }
    }
  }
}
