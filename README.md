# Inventory Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

Inventory management system with ASP.NET Core MVC, REST API, SQL Server, and EF Core.

## 🎯 Overview

Provides a simple inventory workflow for product tracking, stock monitoring, filtering, and reporting.

## ✨ Features

- Dashboard with inventory metrics
- Product CRUD operations
- Search and filtering
- Pagination
- Input validation
- Health monitoring
- API documentation

## 🛠 Tech Stack

| Component | Technology |
|-----------|------------|
| Frontend | ASP.NET Core MVC + Bootstrap 5 |
| Backend | ASP.NET Core Web API |
| Database | SQL Server + Entity Framework Core |
| Validation | FluentValidation |
| Testing | xUnit |

## 🏗 Architecture

```
┌─────────────────┐
│   Web UI      │
├─────────────────┤
│   Controllers   │
├─────────────────┤
│   Services      │
├─────────────────┤
│   Repositories  │
├─────────────────┤
│   Database     │
└─────────────────┘
```

## 🚀 Quick Start

```bash
git clone https://github.com/Lyuben13/InventoryManagementSystem.git
cd InventoryManagementSystem/InventoryEFCoreSQLServerProjectVol2
dotnet restore
dotnet ef database update
dotnet run
```

## 📚 API Endpoints

- `GET /api/products` - List with pagination and filtering
- `GET /api/products/{id}` - Get single product
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update existing product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/report` - Inventory report
- `GET /health` - Health check
- `GET /status` - System status dashboard

## 🧪 Testing

```bash
dotnet test
```

Unit tests are included for the service layer.

## 📁 Project Structure

```
InventoryManagementSystem/
├── InventoryEFCoreSQLServerProjectVol2/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Validators/
│   ├── Views/
│   ├── wwwroot/
│   ├── Migrations/
│   ├── Program.cs
│   └── Inventory.Api.csproj
├── Inventory.Api.Tests/
│   ├── ProductServiceTests.cs
│   └── Inventory.Api.Tests.csproj
└── README.md
```

## 🔧 Configuration

- Database connection string in `appsettings.json`
- EF Core migrations included in project
- FluentValidation for request validation
- Application logging configured through ASP.NET Core logging

## 🎯 Core Functionality

- Create products with automatic ID generation
- Update product information
- Delete products with confirmation
- Search by name or supplier
- Filter by quantity and price range
- Real-time stock monitoring
- Low stock alerts
- Inventory reports

## 🚧 Known Limitations

- No user authentication
- No data export functionality
- Limited sorting options
- Responsive but not mobile-optimized

## 📈 Future Improvements

- User authentication and authorization
- Data export (PDF/Excel)
- Advanced search
- Mobile app
- Docker support
- Real-time updates
- Advanced sorting
- Dark Mode

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

---

Built with .NET 8 and modern web technologies
