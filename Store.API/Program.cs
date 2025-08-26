
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Store.API.Middleware;
using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Services;
using Store.infrastructure;
using Store.infrastructure.Data;
using Store.infrastructure.Repositories;
using System.Text;

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

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

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
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);



builder.Services.AddAuthentication(op =>
{
  op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op=>
{
  op.RequireHttpsMetadata = false;
  op.SaveToken = true;
  op.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    ValidateIssuer = false,
    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidateAudience = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
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
