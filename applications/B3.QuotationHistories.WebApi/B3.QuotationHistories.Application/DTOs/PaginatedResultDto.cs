namespace B3.QuotationHistories.Application.DTOs;

public class PaginatedResultDto<TItem>(
    int pageNumber,
    int pageSize,
    int totalNumberOfItems,
    int totalNumberOfPages,
    IEnumerable<TItem> items)
{
    public int PageNumber { get; set; } = pageNumber;

    public int PageSize { get; set; } = pageSize;

    public int TotalNumberOfItems { get; set; } = totalNumberOfItems;

    public int TotalNumberOfPages { get; set; } = totalNumberOfPages;
    
    public IEnumerable<TItem> Items { get; set; } = items;
}