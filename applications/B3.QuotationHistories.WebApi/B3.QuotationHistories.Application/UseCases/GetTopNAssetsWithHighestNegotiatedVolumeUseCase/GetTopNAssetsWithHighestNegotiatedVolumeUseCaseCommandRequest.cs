using B3.QuotationHistories.Application.Interfaces;

namespace B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;

public class GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest : IUseCaseCommandRequest
{
    public int NumberOfTopAssetsWithHighestNegotiatedVolume { get; set; }
}