using B3.QuotationHistories.Application.Common;
using B3.QuotationHistories.Application.Exceptions;

namespace B3.QuotationHistories.Application.ValueObjects;

public class PageSize
{
    public int Value { get; }

    private PageSize(int value)
    {
        Value = value;
    }

    public static PageSize Create(int value)
    {
        if (value is < PaginationSettings.MinPageSize or > PaginationSettings.MaxPageSize)
        {
            throw new InvalidPageSizeException(
                $"Page size \"{value}\" is invalid. It must be between {PaginationSettings.MinPageSize} and {PaginationSettings.MaxPageSize}");
        }

        return new PageSize(value);
    }

    public static implicit operator int(PageSize pageSize) => pageSize.Value;
}