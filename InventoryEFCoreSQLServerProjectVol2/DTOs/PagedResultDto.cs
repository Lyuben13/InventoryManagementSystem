namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за пагиниран резултат с метаданни за навигация.
/// </summary>
/// <typeparam name="T">Тип на елементите в страницата.</typeparam>
public class PagedResultDto<T>
{
    /// <summary>Елементи в текущата страница.</summary>
    public List<T> Items { get; set; } = new();

    /// <summary>Текущ номер на страница (1-based).</summary>
    public int Page { get; set; }

    /// <summary>Брой елементи на страница.</summary>
    public int PageSize { get; set; }

    /// <summary>Общ брой елементи (без пагинация).</summary>
    public int TotalCount { get; set; }

    /// <summary>Общ брой страници.</summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>Има ли следваща страница.</summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>Има ли предишна страница.</summary>
    public bool HasPreviousPage => Page > 1;
}
