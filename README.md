📦 StockFlow API

StockFlow API is a backend-focused ASP.NET Core Web API designed for managing products, suppliers, orders, and inventory in a structured and scalable way.
It follows clean architecture principles and is built to demonstrate real-world backend development practices.

🚀 Features

CRUD operations for Products, Suppliers, and Orders

Order lifecycle management (create, update, status tracking)

DTO-based request/response models

Entity Framework Core with Code-First Migrations

Centralized Global Exception Handling Middleware

Clean separation of concerns (Controllers, Services, Data, DTOs)

Environment-based configuration (appsettings.json)

Seed data support for initial setup

🛠 Tech Stack

Framework: ASP.NET Core Web API

Language: C#

ORM: Entity Framework Core

Database: SQL Server (configurable)

Architecture: Layered / Clean Architecture

API Style: RESTful APIs

📂 Project Structure
StockFlow API/
│
├── Controllers/        # API endpoints
├── Data/               # DbContext & database configuration
├── DTOs/               # Data Transfer Objects
├── Middleware/         # Global exception handling
├── Migrations/         # EF Core migrations
├── Models/             # Domain models
├── Services/           # Business logic layer
├── Properties/         # Launch settings
│
├── Program.cs          # Application entry point
├── appsettings.json    # Configuration
├── appsettings.Development.json
└── StockFlow API.csproj

⚙️ Setup & Run Locally
1️⃣ Clone the repository
git clone https://github.com/lokesh55000/StockFlow-API.git

2️⃣ Update database connection

Edit appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Your_SQL_Server_Connection_String"
}

3️⃣ Apply migrations
dotnet ef database update

4️⃣ Run the application
dotnet run


API will be available at:

https://localhost:{port}/api

📌 API Modules

Products

Create, update, restock, and fetch products

Suppliers

Manage supplier details and relationships

Orders

Create orders, track status, manage order items

🧠 Learning Objectives Demonstrated

Building production-style REST APIs

Using DTOs to protect domain models

Handling errors globally using middleware

Structuring backend projects for scalability

Working with EF Core Migrations & Seed Data

👨‍💻 Author

Lokesh Bisht
Backend-focused .NET Developer
🔗 GitHub: https://github.com/lokesh55000
