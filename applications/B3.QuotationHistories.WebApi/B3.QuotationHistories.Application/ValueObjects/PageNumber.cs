using B3.QuotationHistories.Application.Exceptions;

namespace B3.QuotationHistories.Application.ValueObjects;

public class PageNumber
{
    public int Value { get; }

    private PageNumber(int value)
    {
        Value = value;
    }

    public static PageNumber Create(int value)
    {
        if (value < 1)
        {
            throw new InvalidPageNumberException(
                $"Page number \"{value}\" is invalid. It must be greater than or equal to 1");
        }

        return new PageNumber(value);
    }

    public static implicit operator int(PageNumber pageNumber) => pageNumber.Value;
}