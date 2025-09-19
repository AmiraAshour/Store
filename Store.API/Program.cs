
using Hangfire;
using QuestPDF.Infrastructure;
using Store.API.Middleware;
using Store.Core;
using Store.Core.Services;
using Store.infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);



builder. Services.CoreConfiguration(builder.Configuration).InfrasturctureConfiguration(builder.Configuration);

StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"];

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();

app.UseHangfireDashboard("/dashboard");

RecurringJob.AddOrUpdate<ReportService>(
    "daily-report-job",
    service => service.SendDailyReportAsync(),
    "* 6 * * *"
);

RecurringJob.AddOrUpdate<ReportService>(
    "monthly-report-job",
    service => service.SendMonthlyReportAsync(),
    "0 9 1 * *" 
);

app.UseCors("CORSPolicy");

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
});


app.UseRateLimiter();

app.MapControllers();



app.Run();


