using Inventory.Api.Models;

namespace Inventory.Api.Repositories;

/// <summary>
/// Контракт за persistence операции върху продукти.
/// Repo се занимава с изграждане/изпълнение на EF заявки, докато Service управлява бизнес поток и кога се прави `SaveChanges`.
/// </summary>
public interface IProductRepository
{
    Task<(List<Product> products, int totalCount)> GetAllAsync(string? search, int? minQuantity, string? supplier, decimal? maxPrice, int page = 1, int pageSize = 20);
    Task<List<Product>> GetAllForReportAsync();
    Task<Product?> GetByIdAsync(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> SaveChangesAsync();
}
