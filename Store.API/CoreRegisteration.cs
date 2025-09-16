using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Entities.EntitySettings;
using Store.Core.Interfaces;
using Store.Core.Services;
using Store.infrastructure.Data;
using Store.infrastructure.Repositories;
using System.Text;
using System.Threading.RateLimiting;

namespace Store.Core
{
  public static class CoreRegisteration
  {
    public static IServiceCollection CoreConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      // Rate Limiter Configuration
      services.AddRateLimiter(options =>
      {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: "global",
        _ => new FixedWindowRateLimiterOptions
        {
          Window = TimeSpan.FromSeconds(10),
          PermitLimit = 5,
          QueueLimit = 2,
          QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        }));

      });

      // Add services to the container.
      services.AddControllers();

      services.AddCors(options =>
      {
        options.AddPolicy("CORSPolicy", builder =>
        builder
        //.AllowAnyOrigin()
        .WithOrigins("http://localhost:7025", "https://localhost:7025", "http://localhost:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            );
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
      services.AddScoped<IWishlistService, WishlistService>();
      services.AddScoped<IAddressService, AddressService>();
      services.AddScoped<IProfileService, ProfileSevice>();
      services.AddScoped<IReportService, ReportService>();  



      services.AddSingleton<IFileProvider>(
           new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
       );

      // Authentication configuretion
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, op =>
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
      })
      .AddGoogle("Google", options =>
      {
        var google = configuration.GetSection("Authentication:Google");
        options.ClientId = google["ClientId"]!;
        options.ClientSecret = google["ClientSecret"]!;
        options.CallbackPath = google["CallbackPath"];

        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.SaveTokens = true;


      });


      // Register the Swagger services
      services.AddEndpointsApiExplorer();

      services.AddMemoryCache();

      services.AddSwaggerGen(c =>
       {
         c.EnableAnnotations();
         c.SwaggerDoc("v1", new OpenApiInfo
         {
           Title = "Store API",
           Version = "v1"
         });
       });

      // register the AutoMapper services
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      // register Fluent Validation
      services.AddFluentValidationAutoValidation();
      services.AddValidatorsFromAssemblyContaining<ProductDTOValidator>();


      services.AddHangfire(config => config.UseSqlServerStorage(configuration.GetConnectionString("Store")));
      services.AddHangfireServer();

      return services;
    }
  }
}
