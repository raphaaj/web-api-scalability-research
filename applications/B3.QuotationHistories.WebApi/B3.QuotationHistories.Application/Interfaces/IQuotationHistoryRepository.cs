using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Domain.Entities;

namespace B3.QuotationHistories.Application.Interfaces;

public interface IQuotationHistoryRepository
{
    Task<QuotationHistoryEntity[]> GetQuotationHistoriesAsync(QuotationHistoryFilterDto quotationHistoryFilter,
        PagingOptionsDto pagingOptions);

    Task<int> GetTotalNumberOfQuotationHistoriesAsync(QuotationHistoryFilterDto quotationHistoryFilter);

    Task<TopAssetWithHighestNegotiatedVolumeDto[]> GetTopNAssetsWithHighestNegotiatedVolumeAsync(int n);
}