using Microsoft.AspNetCore.Identity;
using Store.API.Helper;
using Store.API.Middleware;
using Store.Core;
using Store.Core.Entities;
using Store.infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);



builder. Services.CoreConfiguration(builder.Configuration);
builder.Services.InfrasturctureConfiguration(builder.Configuration);

StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"];


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();
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


