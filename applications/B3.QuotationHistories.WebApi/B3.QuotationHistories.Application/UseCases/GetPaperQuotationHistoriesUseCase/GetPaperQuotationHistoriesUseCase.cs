using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Exceptions;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Application.Mappers;
using B3.QuotationHistories.Application.ValueObjects;
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
        var pageSize = PageSize.Create(useCaseCommandRequest.PageSize);
        var pageNumber = PageNumber.Create(useCaseCommandRequest.PageNumber);
        
        var quotationHistoriesFilter = new QuotationHistoryFilterDto
        {
            PaperNegotiationCode = paperNegotiationCode,
        };

        var totalNumberOfQuotationHistories =
            await quotationHistoryRepository.GetTotalNumberOfQuotationHistoriesAsync(quotationHistoriesFilter);

        var totalNumberOfQuotationHistoriesPages =
            (int)Math.Ceiling((double)totalNumberOfQuotationHistories / pageSize);

        if (pageNumber > totalNumberOfQuotationHistoriesPages)
        {
            throw new InvalidPageNumberException(
                $"Page number \"{pageNumber}\" is invalid. It must be between 1 and {totalNumberOfQuotationHistoriesPages}.");
        }

        var quotationHistoriesPagingOptions = new PagingOptionsDto
        {
            Offset = (useCaseCommandRequest.PageNumber - 1) * useCaseCommandRequest.PageSize,
            Limit = useCaseCommandRequest.PageSize
        };

        var quotationHistories =
            await quotationHistoryRepository.GetQuotationHistoriesAsync(quotationHistoriesFilter,
                quotationHistoriesPagingOptions);

        var quotationHistoriesResults =
            quotationHistories.Select(QuotationHistoryEntityMapper.ToPaperHistoricalQuotationDto).ToArray();

        return new GetPaperQuotationHistoriesUseCaseCommandResult(pageNumber, pageSize, totalNumberOfQuotationHistories,
            totalNumberOfQuotationHistoriesPages, quotationHistoriesResults);
    }
}