using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Infrastructure.Interfaces;
using B3.QuotationHistories.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace B3.QuotationHistories.Infrastructure.Cache;

public class QuotationHistoryCacheInitializer
{
    private readonly ILogger<QuotationHistoryCacheInitializer> _logger;
    private readonly CacheSettings _cacheSettings;
    private readonly IQuotationHistoryRepository _quotationHistoryRepository;
    private readonly IQuotationHistoryCacheService _quotationHistoryCacheService;

    public QuotationHistoryCacheInitializer(
        ILogger<QuotationHistoryCacheInitializer> logger,
        IOptions<CacheSettings> cacheSettingsOptions,
        IQuotationHistoryRepository quotationHistoryRepository,
        IQuotationHistoryCacheService quotationHistoryCacheService)
    {
        _logger = logger;
        _cacheSettings = cacheSettingsOptions.Value;
        _quotationHistoryRepository = quotationHistoryRepository;
        _quotationHistoryCacheService = quotationHistoryCacheService;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Inicializando cache dos ativos com maior volume financeiro negociado");

        var topAssetsWithHighestNegotiatedVolumeToCache =
            await _quotationHistoryRepository.GetTopNAssetsWithHighestNegotiatedVolumeAsync(_cacheSettings
                .NumberOfAssetsToCache);

        _quotationHistoryCacheService.SetTopAssetsWithHighestNegotiatedVolume(
            topAssetsWithHighestNegotiatedVolumeToCache);

        _logger.LogInformation("Inicialização do cache dos ativos com maior volume financeiro negociado finalizada");

        _logger.LogInformation(
            "Inicializando cache do histórico de cotações dos ativos com maior volume financeiro negociado");

        foreach (var topAsset in topAssetsWithHighestNegotiatedVolumeToCache)
        {
            var quotationHistoryEntities =
                await _quotationHistoryRepository.GetQuotationHistoriesByPaperNegotiationCodeAsync(
                    topAsset.PaperNegotiationCode);

            _quotationHistoryCacheService.SetQuotationHistoryEntitiesByPaperNegotiationCode(
                topAsset.PaperNegotiationCode.Value, quotationHistoryEntities);
        }

        _logger.LogInformation(
            "Inicialização do cache do histórico de cotações dos ativos com maior volume financeiro negociado finalizada");
    }
}