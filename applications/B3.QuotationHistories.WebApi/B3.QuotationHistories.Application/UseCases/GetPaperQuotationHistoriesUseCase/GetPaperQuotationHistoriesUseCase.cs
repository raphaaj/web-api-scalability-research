using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Application.Mappers;
using B3.QuotationHistories.Domain.ValueObjects;

namespace B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;

public class GetPaperQuotationHistoriesUseCase(IQuotationHistoryRepository quotationHistoryRepository)
    : IUseCase<GetPaperQuotationHistoriesUseCaseCommandRequest,
        GetPaperQuotationHistoriesUseCaseCommandResult>
{
    public async Task<GetPaperQuotationHistoriesUseCaseCommandResult> ExecuteAsync(
        GetPaperQuotationHistoriesUseCaseCommandRequest useCaseCommandRequest)
    {
        ArgumentNullException.ThrowIfNull(useCaseCommandRequest);

        var paperNegotiationCode = PaperNegotiationCode.Create(useCaseCommandRequest.PaperNegotiationCode);

        var quotationHistories =
            await quotationHistoryRepository.GetQuotationHistoriesByPaperNegotiationCodeAsync(paperNegotiationCode);

        var quotationHistoriesResults = quotationHistories
            .Select(QuotationHistoryEntityMapper.ToPaperHistoricalQuotationDto)
            .ToArray();

        return new GetPaperQuotationHistoriesUseCaseCommandResult(quotationHistoriesResults);
    }
}