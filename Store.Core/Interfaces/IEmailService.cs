using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Interfaces
{
  public interface IEmailService
  {
    Task SendConfirmationEmailAsync(string email,string subject, string message, string token, string clientUrl);
    Task SendEmailAsync(string to, string subject, string body);
  }

 

}
