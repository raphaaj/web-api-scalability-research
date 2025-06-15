using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Domain.ValueObjects;
using B3.QuotationHistories.Infrastructure.Interfaces;
using B3.QuotationHistories.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace B3.QuotationHistories.Infrastructure.Repositories;

public class CachedQuotationHistoryRepository : IQuotationHistoryRepository
{
    private readonly CacheSettings _cacheSettings;
    private readonly IQuotationHistoryRepository _quotationHistoryRepository;
    private readonly IQuotationHistoryCacheService _quotationHistoryCacheService;

    public CachedQuotationHistoryRepository(
        IOptions<CacheSettings> cacheSettingsOptions,
        IQuotationHistoryRepository quotationHistoryRepository,
        IQuotationHistoryCacheService quotationHistoryCacheService)
    {
        _cacheSettings = cacheSettingsOptions.Value;
        _quotationHistoryRepository = quotationHistoryRepository;
        _quotationHistoryCacheService = quotationHistoryCacheService;
    }

    public async Task<QuotationHistoryEntity[]> GetQuotationHistoriesByPaperNegotiationCodeAsync(
        PaperNegotiationCode paperNegotiationCode)
    {
        if (_quotationHistoryCacheService.TryGetQuotationHistoryEntitiesByPaperNegotiationCode(
                paperNegotiationCode.Value, out var quotationHistoryEntities))
            return quotationHistoryEntities!;

        if (!_quotationHistoryCacheService.TryGetPaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume(
                out var paperNegotiationCodesFromTopAssets) ||
            !paperNegotiationCodesFromTopAssets!.Contains(paperNegotiationCode.Value))
            return await _quotationHistoryRepository.GetQuotationHistoriesByPaperNegotiationCodeAsync(
                paperNegotiationCode);

        var quotationHistories =
            await _quotationHistoryRepository.GetQuotationHistoriesByPaperNegotiationCodeAsync(paperNegotiationCode);

        _quotationHistoryCacheService.SetQuotationHistoryEntitiesByPaperNegotiationCode(paperNegotiationCode.Value,
            quotationHistories);

        return quotationHistories;
    }

    public async Task<TopAssetWithHighestNegotiatedVolumeDto[]> GetTopNAssetsWithHighestNegotiatedVolumeAsync(int n)
    {
        if (n > _cacheSettings.NumberOfAssetsToCache)
            return await _quotationHistoryRepository.GetTopNAssetsWithHighestNegotiatedVolumeAsync(n);

        if (_quotationHistoryCacheService.TryGetTopAssetsWithHighestNegotiatedVolume(
                out var topAssetsWithHighestNegotiatedVolumeCached))
            return topAssetsWithHighestNegotiatedVolumeCached!.Take(n).ToArray();

        var topAssetsWithHighestNegotiatedVolume =
            await _quotationHistoryRepository.GetTopNAssetsWithHighestNegotiatedVolumeAsync(_cacheSettings
                .NumberOfAssetsToCache);

        _quotationHistoryCacheService.SetTopAssetsWithHighestNegotiatedVolume(
            topAssetsWithHighestNegotiatedVolume);

        return topAssetsWithHighestNegotiatedVolume.Take(n).ToArray();
    }
}