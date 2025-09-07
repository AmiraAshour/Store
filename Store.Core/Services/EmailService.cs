using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces;
using System.ComponentModel;
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

    public async Task SendConfirmationEmailAsync(string email,string subject, string message, string token, string clientUrl)
    {


      string confirmUrl = $"{clientUrl}?userId={email}&token={Uri.EscapeDataString(token)}";

      string body = $@"
          <html> 
          <head>
          <style>
          .button{{
            border: none;
                border-radius: 10px;
                padding: 15px 30px;
                color: #fff;
                display: inline-block;
                background: linear-gradient(45deg, #ff7e5f, #feb47b);
                cursor: pointer;
                text-decoration: none;
                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
                transition: all 0.3s ease;
                font-size: 16px;
                font-weight: bold;
                font-family: 'Arial', sans-serif;
                animation: glow 1.5s infinite alternate;

          }}
          </style>
          </head>
          <body>
            <h1>{message}</h1>
                    <hr>
                  <br>
            <a class=""button"" href=""{confirmUrl}"">
                   Click here 
            </a>
          </body>
          </html>
                    
                    ";

      await SendEmailAsync(email, subject, body);
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
  }

}
