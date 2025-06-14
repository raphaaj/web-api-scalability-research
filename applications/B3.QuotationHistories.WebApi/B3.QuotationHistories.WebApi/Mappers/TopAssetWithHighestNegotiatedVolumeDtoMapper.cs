using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;
using B3.QuotationHistories.WebApi.Models.Assets;

namespace B3.QuotationHistories.WebApi.Mappers;

public static class TopAssetWithHighestNegotiatedVolumeDtoMapper
{
    public static TopAssetWithHighestNegotiatedVolumeResponse ToTopAssetWithHighestNegotiatedVolumeResponse(
        TopAssetWithHighestNegotiatedVolumeDto topAssetWithHighestNegotiatedVolumeDto)
    {
        return new TopAssetWithHighestNegotiatedVolumeResponse(
            topAssetWithHighestNegotiatedVolumeDto.PaperNegotiationCode,
            topAssetWithHighestNegotiatedVolumeDto.TotalVolumeOfTilesNegotiated);
    }
}