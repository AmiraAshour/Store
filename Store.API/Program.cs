
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Store.API.Middleware;
using Store.Core;
using Store.Core.Interfaces;
using Store.Core.Services;
using Store.infrastructure;
using Store.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
  options.AddPolicy("CORSPolicy", builder =>
  builder.AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

builder.Services.InfrasturctureConfiguration(builder.Configuration);

builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
  var configuration = ConfigurationOptions.Parse("localhost:6379");
  return ConnectionMultiplexer.Connect(configuration);
});

//builder.Services.CoreConfiguration();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IPhotosService, PhotosService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);

// Register the Swagger services
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

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

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseExceptionMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
});


app.UseAuthorization();

app.MapControllers();

app.Run();
