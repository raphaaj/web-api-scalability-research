using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Domain.ValueObjects;

namespace B3.QuotationHistories.Application.Interfaces;

public interface IQuotationHistoryRepository
{
    Task<QuotationHistoryEntity[]> GetQuotationHistoriesByPaperNegotiationCodeAsync(
        PaperNegotiationCode paperNegotiationCode);

    Task<TopAssetWithHighestNegotiatedVolumeDto[]> GetTopNAssetsWithHighestNegotiatedVolumeAsync(int n);
}