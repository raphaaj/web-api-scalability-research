using B3.QuotationHistories.Application.Interfaces;

namespace B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;

public class GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult(
    TopAssetWithHighestNegotiatedVolumeDto[] assets) : IUseCaseCommandResult
{
    public TopAssetWithHighestNegotiatedVolumeDto[] Assets { get; set; } = assets;
}