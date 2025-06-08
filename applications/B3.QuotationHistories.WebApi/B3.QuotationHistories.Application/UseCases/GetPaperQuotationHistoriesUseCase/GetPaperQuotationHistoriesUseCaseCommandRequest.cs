using B3.QuotationHistories.Application.Interfaces;

namespace B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;

public class GetPaperQuotationHistoriesUseCaseCommandRequest : IUseCaseCommandRequest
{
    public string? PaperNegotiationCode { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}