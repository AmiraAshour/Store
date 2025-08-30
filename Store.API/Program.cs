using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Store.API.Middleware;
using Store.Core.Entities;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces;
using Store.Core.Services;
using Store.infrastructure;
using Store.infrastructure.Data;
using Store.infrastructure.Repositories;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
  options.AddPolicy("CORSPolicy", builder =>
  builder.WithOrigins("http://localhost:5500")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"];

builder.Services.InfrasturctureConfiguration(builder.Configuration);


builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
  var configuration = ConfigurationOptions.Parse("localhost:6379");
  return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 6;
  options.Password.RequireLowercase = true;
  options.Password.RequireUppercase = false;
  options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//builder.Services.CoreConfiguration();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IPhotosService, PhotosService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);



builder.Services.AddAuthentication(op =>
{
  op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie().AddJwtBearer(op =>
{
  op.RequireHttpsMetadata = false;
  op.SaveToken = true;
  op.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateAudience = false,
    //ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidateIssuer = false,
    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
  };
  op.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      if (context.Request.Cookies.ContainsKey("token"))
      {
        context.Token = context.Request.Cookies["token"];
      }

      return Task.CompletedTask;
    }
  };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
});



app.MapControllers();

app.Run();
