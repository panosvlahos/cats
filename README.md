# cats

# 🐱 Cat Fetching API

A clean-architecture ASP.NET Core Web API project that fetches cat images and data from [TheCatAPI](https://api.thecatapi.com/v1), stores them in a local SQL Server database, and provides endpoints to query and retrieve cat information.

---

## 🚀 Features

- Fetches 25 random cats from TheCatAPI (via background job).
- Saves image data and tags (parsed from breed temperaments).
- Provides paginated and tag-based querying.
- Built with Clean Architecture principles.
- CQRS (Command Query Responsibility Segregation) with MediatR.
- Background processing using Hangfire.
- AutoMapper for DTO mapping.
- FluentValidation for request validation.
- Swagger UI for interactive API documentation.
- Unit tests with xUnit.

---

## 🛠️ Technologies Used

- ASP.NET Core 8 Web API
- Entity Framework Core + SQL Server
- Hangfire
- AutoMapper
- MediatR (CQRS)
- FluentValidation
- xUnit (for unit testing)
- Swagger / Swashbuckle

---

## 📦 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code

---

### ⚙️ Configuration

1. Clone the repository:

`bash
git clone [https://github.com/your-username/cat-fetching-api.git](https://github.com/panosvlahos/cats.git)

2. create Database 
run DbScript.sql

3. update appsettings
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CatsDb;Trusted_Connection=True;"
  },
  "TheCatApi": {
    "BaseUrl": "https://api.thecatapi.com/v1",
    "ApiKey": "YOUR_API_KEY"
  }
}


