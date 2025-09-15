

namespace Store.Core.Interfaces
{
  public interface IReportService
  {
    Task SendDailyReportAsync();
    Task SendMonthlyReportAsync();
  }
}
