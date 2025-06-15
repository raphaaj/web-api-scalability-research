using B3.QuotationHistories.Application.DTOs;
using B3.QuotationHistories.Domain.Entities;
using B3.QuotationHistories.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace B3.QuotationHistories.Infrastructure.Cache;

public class QuotationHistoryCacheService : IQuotationHistoryCacheService
{
    private readonly ILogger<QuotationHistoryCacheService> _logger;
    private readonly IMemoryCache _memoryCache;

    public QuotationHistoryCacheService(
        ILogger<QuotationHistoryCacheService> logger,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    private static string TopAssetsWithHighestNegotiatedVolumeCacheKey =>
        $"{nameof(QuotationHistoryCacheService)}:TopAssetsWithHighestNegotiatedVolume";

    private static string PaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolumeCacheKey =>
        $"{nameof(QuotationHistoryCacheService)}:PaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume";

    private static string GetQuotationHistoryEntitiesByPaperNegotiationCodeCacheKey(string paperNegotiationCode) =>
        $"{nameof(QuotationHistoryCacheService)}:QuotationHistoryEntities:{paperNegotiationCode}";

    public bool TryGetTopAssetsWithHighestNegotiatedVolume(
        out TopAssetWithHighestNegotiatedVolumeDto[]? topAssetsWithHighestNegotiatedVolume)
    {
        return _memoryCache.TryGetValue(TopAssetsWithHighestNegotiatedVolumeCacheKey,
            out topAssetsWithHighestNegotiatedVolume);
    }

    public bool TryGetPaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume(
        out HashSet<string>? paperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume)
    {
        return _memoryCache.TryGetValue(PaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolumeCacheKey,
            out paperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolume);
    }

    public bool TryGetQuotationHistoryEntitiesByPaperNegotiationCode(string paperNegotiationCode,
        out QuotationHistoryEntity[]? quotationHistories)
    {
        var cacheKey = GetQuotationHistoryEntitiesByPaperNegotiationCodeCacheKey(paperNegotiationCode);

        return _memoryCache.TryGetValue(cacheKey, out quotationHistories);
    }

    public void SetTopAssetsWithHighestNegotiatedVolume(
        TopAssetWithHighestNegotiatedVolumeDto[] topAssetsWithHighestNegotiatedVolume)
    {
        var paperNegotiationCodes = topAssetsWithHighestNegotiatedVolume
            .Select(x => x.PaperNegotiationCode.Value)
            .ToHashSet();

        _memoryCache.Set(TopAssetsWithHighestNegotiatedVolumeCacheKey, topAssetsWithHighestNegotiatedVolume,
            new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove,
                PostEvictionCallbacks = { GetDefaultPostEvictionCallbackRegistration() },
            });

        _memoryCache.Set(PaperNegotiationCodesFromTopAssetsWithHighestNegotiatedVolumeCacheKey, paperNegotiationCodes,
            new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove,
                PostEvictionCallbacks = { GetDefaultPostEvictionCallbackRegistration() },
            });
    }

    public void SetQuotationHistoryEntitiesByPaperNegotiationCode(string paperNegotiationCode,
        QuotationHistoryEntity[] quotationHistoryEntities)
    {
        var cacheKey = GetQuotationHistoryEntitiesByPaperNegotiationCodeCacheKey(paperNegotiationCode);

        _memoryCache.Set(cacheKey, quotationHistoryEntities, new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.NeverRemove,
            PostEvictionCallbacks = { GetDefaultPostEvictionCallbackRegistration() },
        });
    }

    private PostEvictionCallbackRegistration GetDefaultPostEvictionCallbackRegistration()
    {
        return new PostEvictionCallbackRegistration()
        {
            EvictionCallback = (key, _, reason, state) =>
            {
                var logger = state as ILogger<QuotationHistoryCacheService>;

                logger!.LogWarning("A entrada do cache com chave \"{CacheKey}\" foi removida. Motivo: {Reason}", key,
                    reason);
            },
            State = _logger,
        };
    }
}