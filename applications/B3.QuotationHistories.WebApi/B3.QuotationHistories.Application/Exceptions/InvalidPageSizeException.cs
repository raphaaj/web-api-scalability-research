using B3.QuotationHistories.Application.Common;

namespace B3.QuotationHistories.Application.Exceptions;

public class InvalidPageSizeException(string message) : Exception(message)
{
    public int MinPageSize { get; } = PaginationSettings.MinPageSize;

    public int MaxPageSize { get; } = PaginationSettings.MaxPageSize;
}