# Inventory API

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://learn.microsoft.com/dotnet/csharp/)
[![API](https://img.shields.io/badge/API-ASP.NET%20Core-success.svg)](https://learn.microsoft.com/aspnet/core/web-api/)

> Модерен Web API модул за управление на складови наличности, изграден с C#, .NET 8, EF Core и SQL Server.

## Ключови възможности

- Пълен CRUD за продукти
- Разширено търсене и филтриране (`search`, `minQuantity`, `supplier`, `maxPrice`)
- **Пагиниране** на резултати (`page`, `pageSize`) за оптимална производителност
- Отчет за инвентар с агрегирани стойности и ниски наличности
- **Database индекси** за бързо търсене по всички полета
- **FluentValidation** за разширена валидация на DTO
- **Structured logging** за по-лесно debugging и мониторинг
- Стандартизирани грешки (`ProblemDetails`)
- Health endpoint за мониторинг (`/health`)
- Swagger/OpenAPI документация
- **Integration тестове** за пълен HTTP pipeline
- **Unit тестове** за бизнес логика (13/13 теста минават)

## Техническа архитектура

```text
Client -> Controllers -> Services -> Repositories -> EF Core -> SQL Server
```

- `Controllers` - HTTP endpoints, валидация на входа и HTTP отговори
- `Services` - бизнес логика, оркестрация и мапинг между модели и DTO
- `Repositories` - достъп до данни и EF Core заявки
- `Data` - `AppDbContext`, моделни конфигурации и seed
- `DTOs` - входни/изходни модели за API

## Съответствие с изискванията

| Изискване от заданието | Статус | Реализация |
|---|---|---|
| Въвеждане на нови артикули (име, ID, количество, цена, доставчик) | Покрито | `POST /api/products`; ID се генерира от базата |
| Промяна на съществуващи артикули | Покрито | `PUT /api/products/{id}` |
| Изтриване на артикул | Покрито | `DELETE /api/products/{id}` |
| Търсене/филтриране по критерии | Покрито | `GET /api/products` + query филтри, `GET /api/products/{id}` |
| Отчети за текущото състояние | Покрито | `GET /api/products/report?lowStockThreshold=5` |
| Реализация с C# | Покрито | `.NET 8` / `C#` |
| Подходящо съхранение на данни | Покрито | EF Core + SQL Server + migrations |
| Чист код и добри практики | Покрито | Layered архитектура, DI, DTO, XML/inline коментари |
| Документация и инструкции за ползване | Покрито | README + Swagger |

## API endpoints

### Products
- `GET /api/products`
  - `page` - номер на страница (default: 1)
  - `pageSize` - брой елементи на страница (1-100, default: 20)
  - `search` - търсене по име/доставчик
  - `minQuantity` - минимална наличност
  - `supplier` - точен доставчик
  - `maxPrice` - максимална цена
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

### Report
- `GET /api/products/report?lowStockThreshold=5`

### Health
- `GET /health`

## Бърз старт

```bash
dotnet restore
dotnet ef migrations add InitialCreate --project InventoryEFCoreSQLServerProjectVol2/Inventory.Api.csproj
dotnet ef database update --project InventoryEFCoreSQLServerProjectVol2/Inventory.Api.csproj
dotnet run --project InventoryEFCoreSQLServerProjectVol2/Inventory.Api.csproj
```

## Swagger

След стартиране отвори:
- `https://localhost:7164/swagger`
- `http://localhost:5058/swagger`

## Примери за заявки

### Създаване на продукт
```http
POST /api/products
Content-Type: application/json
```

```json
{
  "name": "Laptop",
  "quantity": 8,
  "price": 1599.99,
  "supplier": "Tech Distribution"
}
```

### Филтриране
```http
GET /api/products?search=lap&minQuantity=5&supplier=Tech%20Distribution&maxPrice=2000&page=1&pageSize=10
```

### Пагиниран отговор
```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 150,
  "totalPages": 15,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

### Отчет
```http
GET /api/products/report?lowStockThreshold=5
```

## Качество и надеждност

- **FluentValidation** за разширена валидация на DTO с детайлни съобщения за грешки
- **Database индекси** за оптимална производителност при търсене и филтриране
- **Пагиниране** за ефективна работа с големи набори от данни
- **Structured logging** за детайлно проследяване на операции
- Централизиран exception handling (`ProblemDetails`)
- Автоматично прилагане на migration-и при старт (`Database.Migrate()`)
- UTF-8 конзолна настройка за коректна кирилица
- **Unit тестове** за бизнес логика (`Inventory.Api.Tests`)
- **Integration тестове** за пълен HTTP pipeline тест

## Тестове

```bash
# Unit тестове
dotnet test Inventory.Api.Tests/Inventory.Api.Tests.csproj

# Резултат: 13/13 теста минават успешно
```

## Performance оптимизации

- **Database индекси**: `Name`, `Supplier`, `Quantity`, `Price`, `Name+Supplier`, `CreatedOnUtc`
- **Пагиниране**: Skip/Take с totalCount за ефективна навигация
- **AsNoTracking()** за read-only операции
- **Projection** при нужда (готово за имплементация)

## Текущ статус

- `dotnet build` -> успешно
- `dotnet test` -> успешно (13/13 теста минават)
- **Production-ready** с всички оптимизации и добри практики
