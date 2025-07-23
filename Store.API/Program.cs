
using Microsoft.OpenApi.Models;
using Store.API.Middleware;
using Store.infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
object value = builder.Services.InfrasturctureConfiguration(builder.Configuration);

// Register the Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Store API",
    Version = "v1"
  });
});

// register the AutoMapper services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionMiddleware();
app.UseHttpsRedirection();


  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
  });



app.UseAuthorization();

app.MapControllers();

app.Run();
