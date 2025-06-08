using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;

namespace B3.QuotationHistories.Application.Mappers;

public static class TopAssetWithHighestNegotiatedVolumeMapper
{
    public static
        TopAssetWithHighestNegotiatedVolumeDto ToTopAssetWithHighestNegotiatedVolumeDto(
            DTOs.TopAssetWithHighestNegotiatedVolumeDto
                topAssetWithHighestNegotiatedVolumeDto)
    {
        return new TopAssetWithHighestNegotiatedVolumeDto(
            topAssetWithHighestNegotiatedVolumeDto.PaperNegotiationCode.Value,
            topAssetWithHighestNegotiatedVolumeDto.TotalVolumeOfTilesNegotiated);
    }
}