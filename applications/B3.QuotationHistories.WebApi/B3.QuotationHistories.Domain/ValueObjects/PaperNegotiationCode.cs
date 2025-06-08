using System.Text.RegularExpressions;
using B3.QuotationHistories.Domain.Exceptions;

namespace B3.QuotationHistories.Domain.ValueObjects;

public partial class PaperNegotiationCode
{
    [GeneratedRegex(@"^[A-Z0-9]{5,12}$")]
    private static partial Regex ValidationRegex();

    public string Value { get; }

    private PaperNegotiationCode(string value)
    {
        Value = value;
    }

    public static PaperNegotiationCode Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPaperNegotiationCodeException("Paper negotiation code cannot be null or empty");

        if (!ValidationRegex().IsMatch(value))
            throw new InvalidPaperNegotiationCodeException(
                $"Paper negotiation code \"{value}\" is invalid. It must consist of 5 to 12 uppercase alphanumeric characters");

        return new PaperNegotiationCode(value);
    }

    public override bool Equals(object? obj) =>
        obj is PaperNegotiationCode other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}