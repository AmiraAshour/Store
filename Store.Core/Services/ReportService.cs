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
      var totalOrders = orders?.Count();
      var totalRevenue = orders?.Sum(o => o.GetTotal());

      var newUsers =  _profileService.GetNewUsersForToday();
      var totalNewUsers = newUsers?.Count();

      var pdf = ReportPdfGenerator.GenerateDailyReport(DateTime.UtcNow, totalOrders, totalRevenue, totalNewUsers);

      _logger.LogInformation("📧 Starting Daily Report job at {time}", DateTime.UtcNow);

      await _emailService.SendEmailWithAttachmentAsync(
          "a4dmin746170@gmail.com",
          "📊 Daily Report",
          "Please find attached the daily report.",
          pdf,
          $"DailyReport-{DateTime.UtcNow:yyyyMMdd}.pdf"
      );
      _logger.LogInformation("✅ Daily Report email sent");

    }

    public async Task SendMonthlyReportAsync()
    {
      var orders = await _orderService.GetOrdersForThisMonthAsync();
      var totalOrders = orders?.Count();
      var totalRevenue = orders?.Sum(o => o.GetTotal());

      var newUsers = _profileService.GetNewUsersForThisMonth();
      var totalNewUsers = newUsers?.Count();

      var pdf = ReportPdfGenerator.GenerateMonthlyReport(DateTime.UtcNow, totalOrders, totalRevenue, totalNewUsers);

      await _emailService.SendEmailWithAttachmentAsync(
          "a4dmin746170@gmail.com",
          "📊 Monthly Report",
          "Please find attached the monthly report.",
          pdf,
          $"MonthlyReport-{DateTime.UtcNow:yyyyMM}.pdf"
      );
    }
  }
}
