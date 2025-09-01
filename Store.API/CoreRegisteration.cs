using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.Core.Services;
using Store.infrastructure.Data;
using Store.infrastructure.Repositories;
using System.Text;

namespace Store.Core
{
  public static class CoreRegisteration
  {
    public static IServiceCollection CoreConfiguration(this IServiceCollection services, IConfiguration configuration)
    {


      // Add services to the container.
      services.AddControllers();

      services.AddCors(options =>
      {
        options.AddPolicy("CORSPolicy", builder =>
        builder.WithOrigins("http://localhost:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
      });

      // configure strongly typed settings objects
      services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
      services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

      // configure redis
      services.AddSingleton<IConnectionMultiplexer>(i =>
      {
        var configuration = ConfigurationOptions.Parse("localhost:6379");
        return ConnectionMultiplexer.Connect(configuration);
      });

      // For Identity 
      services.AddIdentity<AppUser, IdentityRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
      }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

      // Add Services 
      services.AddScoped<ICategoriesService, CategoriesService>();
      services.AddScoped<IProductsService, ProductsService>();
      services.AddScoped<IPhotosService, PhotosService>();
      services.AddScoped<IBasketService, BasketService>();
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IPaymentService, PaymentService>();
      services.AddScoped<IReviewService, ReviewService>();



      services.AddSingleton<IFileProvider>(
           new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
       );

      // Authentication configuretion
      services.AddAuthentication(op =>
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
          //ValidAudience = configuration["Jwt:Audience"],
          ValidateIssuer = false,
          //ValidIssuer = configuration["Jwt:Issuer"],
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
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
      services.AddEndpointsApiExplorer();

      services.AddMemoryCache();

      services.AddSwaggerGen(c =>
       {
         c.SwaggerDoc("v1", new OpenApiInfo
         {
           Title = "Store API",
           Version = "v1"
         });
       });

      // register the AutoMapper services
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      return services;
    }
  }
}
