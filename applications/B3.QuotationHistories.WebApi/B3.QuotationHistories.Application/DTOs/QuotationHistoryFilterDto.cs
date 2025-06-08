using B3.QuotationHistories.Domain.ValueObjects;

namespace B3.QuotationHistories.Application.DTOs;

public class QuotationHistoryFilterDto
{
    public PaperNegotiationCode? PaperNegotiationCode { get; set; }
}