# Inventory Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

**Complete inventory management system with web UI and REST API** built with modern .NET technologies.

## 🎯 What It Solves

Manages warehouse inventory with real-time stock tracking, product management, and automated reporting for small to medium businesses.

## ✨ Key Features

- **📊 Real-time Dashboard** - Live KPI metrics and stock alerts
- **🔍 Advanced Search** - Filter by name, supplier, quantity, price
- **📝 Full CRUD Operations** - Create, read, update, delete products
- **📄 Pagination** - Efficient handling of large datasets
- **📋 Inventory Reports** - Automated stock analysis and alerts
- **🔒 Input Validation** - FluentValidation with detailed error messages
- **🏥 Health Monitoring** - System status and health checks
- **📚 API Documentation** - Complete Swagger/OpenAPI spec

## 🛠 Tech Stack

| Component | Technology |
|-----------|------------|
| **Frontend** | ASP.NET Core MVC + Bootstrap 5 |
| **Backend** | ASP.NET Core Web API |
| **Database** | SQL Server + Entity Framework Core |
| **Validation** | FluentValidation |
| **Testing** | xUnit (13/13 tests passing) |
| **Documentation** | Swagger/OpenAPI 3.0 |

## 🏗 Architecture

```
┌─────────────────┐
│   Web UI     │  ← Bootstrap 5 + JavaScript
├─────────────────┤
│   Controllers  │  ← HTTP endpoints + validation
├─────────────────┤
│   Services    │  ← Business logic + orchestration
├─────────────────┤
│ Repositories │  ← Data access + EF Core
├─────────────────┤
│   Database    │  ← SQL Server + migrations
└─────────────────┘
```

## 📸 Screenshots

### 📊 Dashboard
![Dashboard](https://github.com/Lyuben13/InventoryManagementSystem/blob/main/screenshots/dashboard.png)

### 📦 Products Management
![Products](https://github.com/Lyuben13/InventoryManagementSystem/blob/main/screenshots/products.png)

### 🏥 System Status
![Status](https://github.com/Lyuben13/InventoryManagementSystem/blob/main/screenshots/status.png)

### 📚 API Documentation
![Swagger](https://github.com/Lyuben13/InventoryManagementSystem/blob/main/screenshots/swagger.png)

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/Lyuben13/InventoryManagementSystem.git

# Navigate to project
cd InventoryManagementSystem/InventoryEFCoreSQLServerProjectVol2

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

## 📚 API Endpoints

### Products
- `GET /api/products` - List with pagination and filtering
- `GET /api/products/{id}` - Get single product
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update existing product
- `DELETE /api/products/{id}` - Delete product

### Reports
- `GET /api/products/report` - Inventory summary with low stock alerts

### System
- `GET /health` - Health check for monitoring
- `GET /status` - System status dashboard

## 🧪 Testing

```bash
# Run unit tests
dotnet test

# Test results
✅ 13/13 tests passing
⚡ 4 seconds execution time
```

## 📁 Project Structure

```
InventoryEFCoreSQLServerProjectVol2/
├── Controllers/          # HTTP API endpoints
├── Services/             # Business logic layer
├── Repositories/         # Data access layer
├── DTOs/                # Data transfer objects
├── Models/               # Database entities
├── Views/                # MVC Razor views
├── wwwroot/              # Static assets (CSS, JS, images)
├── Validators/            # FluentValidation rules
├── Migrations/           # Database migrations
└── Tests/                # Unit and integration tests
```

## 🔧 Configuration

- **Database:** SQL Server (configurable via `appsettings.json`)
- **Logging:** Structured logging with Serilog
- **Validation:** FluentValidation with custom error messages
- **Pagination:** Configurable page size (1-100 items)
- **CORS:** Configured for development environment

## 🎯 Core Functionality

### Product Management
- ✅ Create products with automatic ID generation
- ✅ Update product information (name, quantity, price, supplier)
- ✅ Delete products with confirmation
- ✅ Search by name or supplier
- ✅ Filter by quantity range and price range

### Inventory Tracking
- ✅ Real-time stock level monitoring
- ✅ Low stock alerts (configurable threshold)
- ✅ Total inventory value calculation
- ✅ Product count and unit tracking

### Reporting & Analytics
- ✅ Inventory summary dashboard
- ✅ Low stock product reports
- ✅ Total value and quantity metrics
- ✅ Supplier-based analysis

## 🚧 Known Limitations

- **Authentication:** No user authentication/authorization
- **Data Export:** No PDF/Excel export functionality
- **Sorting:** Limited sorting options
- **Mobile:** Responsive but not mobile-optimized
- **Performance:** Not load-tested for high traffic

## 📈 Future Improvements

- **🔐 User Authentication** - Role-based access control
- **📊 Advanced Analytics** - Charts and trend analysis
- **📄 Data Export** - PDF reports and Excel export
- **🔍 Enhanced Search** - Full-text search with indexing
- **📱 Mobile App** - Native mobile application
- **🐳 Docker Support** - Containerized deployment
- **🔄 Real-time Updates** - SignalR for live updates
- **📧 Advanced Sorting** - Multi-column sorting
- **🎨 Dark Mode** - Theme switching (partially implemented)

## 🧪 Test Coverage

- **Unit Tests:** 13/13 passing (100%)
- **Integration Tests:** HTTP pipeline testing
- **Fake Repository:** In-memory testing without database
- **Business Logic:** Complete coverage of service layer

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## 📞 Contact

- **Repository:** https://github.com/Lyuben13/InventoryManagementSystem
- **Live Demo:** Contact for deployment access
- **Issues:** Use GitHub Issues for bug reports

---

**Built with ❤️ using .NET 8 and modern web technologies**
