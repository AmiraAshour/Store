
using Microsoft.Extensions.Options;
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
    public EmailService( IOptions<EmailSettings> emailSettings)
    {
      _emailSettings = emailSettings.Value;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
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

        await client.SendMailAsync(mailMessage);
      }
    }

    public async Task SendConfirmationEmailAsync(string email, string userId, string token, string clientUrl)
    {
      string confirmUrl = $"{clientUrl}?userId={userId}&token={Uri.EscapeDataString(token)}";

      string body = $@"
            <html>
            <head>
              <style>
                .button {{
                   border: none;
                   border-radius: 8px;
                   padding: 12px 25px;
                   color: #fff;
                   background: linear-gradient(45deg, #4facfe, #00f2fe);
                   cursor: pointer;
                   text-decoration: none;
                   font-size: 15px;
                   font-weight: bold;
                   font-family: Arial, sans-serif;
                   box-shadow: 0 3px 12px rgba(0,0,0,0.2);
                }}
                h2 {{ color: #333; font-family: Arial; }}
                p {{ font-size: 14px; color: #555; }}
              </style>
            </head>
            <body>
              <h2>Welcome to MyApp 🎉</h2>
              <p>Please confirm your email by clicking the button below:</p>
              <a class=""button"" href=""{confirmUrl}"">Confirm Email</a>
              <p>If you didn’t request this, you can safely ignore this email.</p>
            </body>
            </html>";

      await SendEmailAsync(email, "Confirm your email", body);
    }

    public async Task SendResetPasswordEmailAsync(string email, string userId, string token, string clientUrl)
    {
      string resetUrl = $"{clientUrl}?userId={userId}&token={Uri.EscapeDataString(token)}";

      string body = $@"
            <html>
            <head>
              <style>
                .button {{
                   border: none;
                   border-radius: 8px;
                   padding: 12px 25px;
                   color: #fff;
                   background: linear-gradient(45deg, #ff6a00, #ee0979);
                   cursor: pointer;
                   text-decoration: none;
                   font-size: 15px;
                   font-weight: bold;
                   font-family: Arial, sans-serif;
                   box-shadow: 0 3px 12px rgba(0,0,0,0.2);
                }}
                h2 {{ color: #333; font-family: Arial; }}
                p {{ font-size: 14px; color: #555; }}
              </style>
            </head>
            <body>
              <h2>Password Reset Request 🔑</h2>
              <p>Click the button below to reset your password:</p>
              <a class=""button"" href=""{resetUrl}"">Reset Password</a>
              <p>If you didn’t request a password reset, please ignore this email.</p>
            </body>
            </html>";

      await SendEmailAsync(email, "Reset your password", body);
    }


    public async Task SendOrderInvoiceEmailAsync(string to, Orders order)
    {
      string body = $@"
    <html>
      <head>
        <style>
          body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
          }}
          .container {{
            max-width: 600px;
            margin: 30px auto;
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
          }}
          h2 {{
            color: #2c3e50;
            text-align: center;
          }}
          .order-info, .footer {{
            margin: 20px 0;
            font-size: 14px;
            color: #555;
          }}
          .order-items {{
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
          }}
          .order-items th, .order-items td {{
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
          }}
          .order-items th {{
            background-color: #f8f8f8;
          }}
          .total {{
            font-size: 16px;
            font-weight: bold;
            text-align: right;
            margin-top: 20px;
          }}
          .highlight {{
            color: #27ae60;
            font-weight: bold;
          }}
          .footer {{
            text-align: center;
            font-size: 12px;
            color: #aaa;
          }}
        </style>
      </head>
      <body>
        <div class='container'>
          <h2>Thank you for your order! 🎉</h2>
          <div class='order-info'>
            <p><b>Order Number:</b> #{order.Id}</p>
            <p><b>Order Date:</b> {order.OrderDate:dd MMM yyyy}</p>
            <p><b>Status:</b> <span class='highlight'>{order.status}</span></p>
          </div>

          <h3>Order Summary</h3>
          <table class='order-items'>
            <thead>
              <tr>
                <th>Product</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Subtotal</th>
              </tr>
            </thead>
            <tbody>
              {string.Join("", order.orderItems.Select(item => $@"
                <tr>
                  <td>{item.ProductName}</td>
                  <td>{item.Quntity}</td>
                  <td>{item.Price:C}</td>
                  <td>{(item.Quntity * item.Price):C}</td>
                </tr>"))}
            </tbody>
          </table>

          <p><b>Delivery Method:</b> {order.deliveryMethod.Name} - {order.deliveryMethod.Price:C}</p>
          <p class='total'>Total: {order.GetTotal():C}</p>

          <p style='margin-top:30px;'>We appreciate your business 💙<br/>If you have any questions, feel free to reply to this email.</p>
          
          <div class='footer'>
            <p>&copy; {DateTime.Now.Year} MyApp Inc. All rights reserved.</p>
          </div>
        </div>
      </body>
    </html>";

      await SendEmailAsync(to, $"Invoice for Order #{order.Id}", body);
    }

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
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

        await client.SendMailAsync(mailMessage);
      }
    }


  }

}
