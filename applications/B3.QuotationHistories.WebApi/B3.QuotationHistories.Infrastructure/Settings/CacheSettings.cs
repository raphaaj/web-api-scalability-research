namespace B3.QuotationHistories.Infrastructure.Settings;

public class CacheSettings
{
    public bool UseCacheForTopAssetsWithHighestNegotiatedVolume { get; set; }

    public int NumberOfAssetsToCache { get; set; }
}