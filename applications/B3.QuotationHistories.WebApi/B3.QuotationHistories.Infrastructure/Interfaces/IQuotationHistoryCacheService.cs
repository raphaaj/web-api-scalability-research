using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Domain.Entities;

namespace B3.QuotationHistories.Infrastructure.Interfaces;

public interface IQuotationHistoryCacheService
{
    bool TryGetTopAssetsWithHighestNegotiatedVolume(
        out TopAssetWithHighestNegotiatedVolumeDto[]? topAssetsWithHighestNegotiatedVolume);

    bool TryGetPaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume(
        out HashSet<string>? paperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume);

    bool TryGetQuotationHistoryEntitiesByPaperNegotiationCode(string paperNegotiationCode,
        out QuotationHistoryEntity[]? quotationHistories);

    void SetTopAssetsWithHighestNegotiatedVolume(
        TopAssetWithHighestNegotiatedVolumeDto[] topAssetsWithHighestNegotiatedVolume);

    void SetQuotationHistoryEntitiesByPaperNegotiationCode(string paperNegotiationCode,
        QuotationHistoryEntity[] quotationHistoryEntities);
}