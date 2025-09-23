# Store API

Store API is a robust e-commerce backend built with ASP.NET Core (.NET 8), designed to support modern web and mobile store applications. It provides comprehensive features for product management, user authentication, order processing, payments, reviews, wishlists, reporting, and more.

---

## Features

- **User Authentication & Authorization**
  - JWT-based authentication
  - Google external login
  - Role-based access (Admin, User)
  - Email confirmation and password reset

- **Product Management**
  - CRUD operations for products and categories
  - Photo upload and management
  - Fuzzy and FreeText search for products
  - Product reviews and ratings

- **Order & Basket**
  - Basket management (add, update, remove, clear)
  - Order creation and tracking
  - Delivery methods with admin protection

- **Wishlist**
  - Add/remove products to user wishlist
  - Retrieve wishlist items

- **Address & Profile**
  - Manage user addresses
  - Edit profile, change password, delete account

- **Payment Integration**
  - Stripe payment support
  - Payment status tracking

- **Reporting**
  - Automated daily and monthly PDF reports (QuestPDF)
  - Email delivery to admins
  - Hangfire for background job scheduling

- **Rate Limiting & Caching**
  - Global rate limiting for API protection
  - In-memory caching for performance

- **Validation & Documentation**
  - FluentValidation for all entities and DTOs
  - Swagger/OpenAPI documentation with endpoint descriptions

---

## Technologies Used

- **.NET 8 / ASP.NET Core**
- **Entity Framework Core**
- **Hangfire** (background jobs)
- **Stripe** (payments)
- **QuestPDF** (PDF reports)
- **AutoMapper** (object mapping)
- **FluentValidation** (model validation)
- **Swagger** (API documentation)
- **StackExchange.Redis** (caching)
- **FuzzySharp** (fuzzy search)
- **Microsoft Identity** (user management)

---

## Documentation

- **Swagger UI** ? [https://my-store.runasp.net/swagger/index.html](https://my-store.runasp.net/swagger/index.html)  
  Interactive API documentation with ability to test endpoints.

- **ERD (Entity Relationship Diagram)**  [https://www.mermaidchart.com/app/projects/e83481e1-0ccf-49a4-a8c4-a78e722e996f/diagrams/e6f2cc00-dae2-46ad-981b-d57e32488218/version/v0.1/edit] (https://www.mermaidchart.com/app/projects/e83481e1-0ccf-49a4-a8c4-a78e722e996f/diagrams/e6f2cc00-dae2-46ad-981b-d57e32488218/version/v0.1/edit)
  The database schema is documented using **Mermaid** chart for better visualization:

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Stripe Account](https://dashboard.stripe.com/register) (for payments)
- [Google Developer Console](https://console.developers.google.com/) (for Google login)

### Setup

1. **Clone the repository**
   - git clone https://github.com/AmiraAshour/Store.g  it cd store-api


2. **Configure appsettings.json**
   - Add your connection strings, Stripe keys, Google OAuth credentials, and email settings.

3. **Run database migrations**
  - dotnet ef database update
  
4. **Start Redis server** (if using caching)

5. **Run the application**
  - dotnet run

6. **Access Swagger UI**
- Navigate to `https://localhost:7025/swagger` for interactive API documentation.

7. **Hangfire Dashboard**
- Navigate to `/dashboard` for background job monitoring.

---

## API Endpoints

- **Account:** `/api/account`  
Register, login, Google login, email confirmation, password reset, profile management

- **Products:** `/api/products`  
CRUD, search, reviews, photos

- **Categories:** `/api/categories`  
CRUD

- **Orders:** `/api/orders`  
Create, get, delivery methods (admin only)

- **Basket:** `/api/basket`  
Add, update, remove, clear items

- **Wishlist:** `/api/wishlist`  
Add/remove/get wishlist items

- **Address:** `/api/address`  
Add, update, delete, get addresses

- **Payment:** `/api/payment`  
Stripe payment processing

- **Reports:** `/api/reports`  
Daily/monthly PDF reports (admin only)

```


## Project Structure

Store.API/
 ├── Controllers/           # API controllers (Account, Products, Orders, etc.)
 ├── Middleware/            # Custom middleware (exception handling, logging, etc.)
 ├── Properties/            # Project properties and settings
 ├── Program.cs             # Application entry point and configuration
 ├── appsettings.json       # Main configuration file

Store.Core/
 ├── DTO/                   # Data Transfer Objects
 ├── Entities/              # Domain entities (Product, Category, User, Order, etc.)
 ├── Interfaces/            # Service and repository interfaces
 ├── Services/              # Business logic / service implementations
 ├── Shared/                # Shared utilities, enums, helpers
 ├── FluentValidation/      # Validators for entities and DTOs

Store.Infrastructure/
 ├── Data/                  # DbContext, migrations, database seeding
 ├── Repositories/          # Repository implementations
 ├── Config/                # Entity configurations

wwwroot/
 ├── images/                # Static files (uploads, product photos, etc.)

README.md                   # Project documentation

```

## Contributing

Contributions are welcome!  
- Fork the repo
- Create a feature branch
- Submit a pull request

---

## License

This project is licensed under the MIT License.

---

## Contact

For questions or support, open an issue or contact amira3shour74@gmail.com (mailto:amira3shour74@gmail.com).

---
