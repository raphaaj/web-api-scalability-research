using B3.QuotationHistories.Application.Interfaces;

namespace B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;

public class GetPaperQuotationHistoriesUseCaseCommandResult(PaperQuotationHistoryDto[] quotationHistories)
    : IUseCaseCommandResult
{
    public PaperQuotationHistoryDto[] QuotationHistories { get; set; } = quotationHistories;
}