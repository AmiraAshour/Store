using Microsoft.Extensions.Logging;
using Store.Core.Interfaces;

namespace Store.Core.Services
{
  public class ReportService: IReportService
  {
    private readonly IOrderService _orderService;
    private readonly IEmailService _emailService;
    private readonly IProfileService _profileService;
    private readonly ILogger<IReportService> _logger;
    public ReportService(IEmailService emailService, IOrderService orderService, IProfileService profileService, ILogger<IReportService> logger)
    {
      _emailService = emailService;
      _orderService = orderService;
      _profileService = profileService;
      _logger = logger;
    }
    public async Task SendDailyReportAsync()
    {
      var orders = await _orderService.GetOrdersForTodayAsync();

      var newUsers =  _profileService.GetNewUsersForToday();
      var totalNewUsers = newUsers?.Count();

      var pdf = ReportPdfGenerator.GenerateDailyReport(DateTime.UtcNow, orders, totalNewUsers);

      _logger.LogInformation("📧 Starting Daily Report job at {time}", DateTime.UtcNow);
      var adminEmails = await _profileService.GetEmailsAdmin();
      if(adminEmails is null || !adminEmails.Any())
      {
        _logger.LogWarning("⚠️ No admin emails found. Daily Report email not sent.");
        return;
      }
      _logger.LogInformation("📧 Starting Monthly Report job at {time}", DateTime.UtcNow);  

      foreach (var item in adminEmails)
      {

        await _emailService.SendEmailWithAttachmentAsync(
         item!,
          "📊 Daily Report",
          "Please find attached the daily report.",
          pdf,
          $"DailyReport-{DateTime.UtcNow:yyyyMMdd}.pdf"
      );
      }
      _logger.LogInformation("✅ Daily Report email sent");

    }

    public async Task SendMonthlyReportAsync()
    {
      var orders = await _orderService.GetOrdersForThisMonthAsync();

      var newUsers = _profileService.GetNewUsersForThisMonth();
      var totalNewUsers = newUsers?.Count();

      var pdf = ReportPdfGenerator.GenerateMonthlyReport(DateTime.UtcNow, orders, totalNewUsers);
      var adminEmails = await _profileService.GetEmailsAdmin();
      if(adminEmails is null || !adminEmails.Any())
      {
        _logger.LogWarning("⚠️ No admin emails found. Monthly Report email not sent.");
        return;
      }
      _logger.LogInformation("📧 Starting Monthly Report job at {time}", DateTime.UtcNow);

      foreach (var item in adminEmails)
      {
        
      await _emailService.SendEmailWithAttachmentAsync(
          item!,
          "📊 Monthly Report",
          "Please find attached the monthly report.",
          pdf,
          $"MonthlyReport-{DateTime.UtcNow:yyyyMM}.pdf"
      );
      }
    }
  }
}
