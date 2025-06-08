namespace B3.QuotationHistories.WebApi.Models;

public class PaginatedResponse<TItem>(
    int pageNumber,
    int pageSize,
    int totalNumberOfItems,
    int totalNumberOfPages,
    TItem[] items)
{
    public TItem[] Items { get; init; } = items;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public int TotalNumberOfItems { get; init; } = totalNumberOfItems;
    public int TotalNumberOfPages { get; init; } = totalNumberOfPages;
}