namespace Store.Core.Interfaces.ServiceInterfaces
{
  public interface IReportService
  {
    Task SendDailyReportAsync();
    Task SendMonthlyReportAsync();
  }
}
