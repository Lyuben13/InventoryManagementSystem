using Inventory.Api.Data;
using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repositories;

/// <summary>
/// EF Core repository за продукти.
/// Тук се изграждат LINQ заявки и се изпълняват към базата (без бизнес правила).
/// </summary>
public class ProductRepository : IProductRepository
{
    // DbContext е unit-of-work за текущата DI scope.
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Връща продукти с опционални филтри и paging:
    /// - `search`: филтрира по `Name` или `Supplier`
    /// - `minQuantity`: минимално количество
    /// - `supplier`: точен доставчик
    /// - `maxPrice`: максимална цена
    /// - `page`: номер на страница (1-based)
    /// - `pageSize`: брой елементи на страница
    /// </summary>
    public async Task<(List<Product> products, int totalCount)> GetAllAsync(
        string? search, 
        int? minQuantity, 
        string? supplier, 
        decimal? maxPrice,
        int page = 1,
        int pageSize = 20)
    {
        IQueryable<Product> query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                EF.Functions.Like(p.Name, $"%{term}%") ||
                EF.Functions.Like(p.Supplier, $"%{term}%"));
        }

        if (minQuantity.HasValue)
        {
            query = query.Where(p => p.Quantity >= minQuantity.Value);
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            var supplierTerm = supplier.Trim();
            query = query.Where(p => p.Supplier == supplierTerm);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        var totalCount = await query.CountAsync();
        
        var products = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    /// <summary>
    /// Връща всички продукти без paging за генериране на отчети.
    /// </summary>
    public async Task<List<Product>> GetAllForReportAsync() =>
        await _context.Products.AsNoTracking().ToListAsync();

    /// <summary>
    /// Връща продукт по ID или `null`, ако няма такъв.
    /// </summary>
    public Task<Product?> GetByIdAsync(int id) =>
        _context.Products.FirstOrDefaultAsync(p => p.Id == id);

    /// <summary>
    /// Добавя нов продукт към контекста.
    /// Реалното записване става при извикване на `SaveChangesAsync` от Service слоя.
    /// </summary>
    public void Add(Product product) => _context.Products.Add(product);

    /// <summary>
    /// Маркира продукт за обновяване.
    /// EF Core change tracking ще открие промените автоматично при SaveChanges.
    /// </summary>
    public void Update(Product product) => _context.Products.Update(product);

    /// <summary>
    /// Маркира продукт за изтриване.
    /// Реалното изтриване става при `SaveChangesAsync`.
    /// </summary>
    public void Delete(Product product) => _context.Products.Remove(product);

    /// <summary>
    /// Записва промените в базата и връща дали поне 1 ред е засегнат.
    /// </summary>
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}
