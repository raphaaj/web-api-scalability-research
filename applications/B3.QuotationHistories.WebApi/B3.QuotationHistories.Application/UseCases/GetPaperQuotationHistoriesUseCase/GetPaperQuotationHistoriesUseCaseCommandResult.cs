using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Interfaces;

namespace B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;

public class GetPaperQuotationHistoriesUseCaseCommandResult(
    int pageNumber,
    int pageSize,
    int totalNumberOfItems,
    int totalNumberOfPages,
    IEnumerable<PaperQuotationHistoryDto> items)
    : PaginatedResultDto<PaperQuotationHistoryDto>(pageNumber, pageSize, totalNumberOfItems, totalNumberOfPages,
        items), IUseCaseCommandResult
{
}