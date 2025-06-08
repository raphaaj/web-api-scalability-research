using B3.QuotationHistories.Application.Exceptions;
using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Application.Mappers;

namespace B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;

public class GetTopNAssetsWithHighestNegotiatedVolumeUseCase(IQuotationHistoryRepository quotationHistoryRepository)
    : IUseCase<GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest,
        GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult>
{
    public const int MinNumberOfTopAssetsWithHighestNegotiatedVolume = 1;
    public const int MaxNumberOfTopAssetsWithHighestNegotiatedVolume = 1000;

    public async Task<GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult> ExecuteAsync(
        GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest commandRequest)
    {
        if (commandRequest.NumberOfTopAssetsWithHighestNegotiatedVolume is <
            MinNumberOfTopAssetsWithHighestNegotiatedVolume or > MaxNumberOfTopAssetsWithHighestNegotiatedVolume)
        {
            throw new InvalidNumberOfTopAssetsWithHighestNegotiatedVolumeException(
                $"Number of top assets with highest negotiated volume is invalid. It must be between " +
                $"{MinNumberOfTopAssetsWithHighestNegotiatedVolume} and {MaxNumberOfTopAssetsWithHighestNegotiatedVolume}",
                MinNumberOfTopAssetsWithHighestNegotiatedVolume, MaxNumberOfTopAssetsWithHighestNegotiatedVolume);
        }

        var topNAssetsWithHighestNegotiatedVolume =
            await quotationHistoryRepository.GetTopNAssetsWithHighestNegotiatedVolumeAsync(commandRequest
                .NumberOfTopAssetsWithHighestNegotiatedVolume);

        var assets = topNAssetsWithHighestNegotiatedVolume
            .Select(TopAssetWithHighestNegotiatedVolumeMapper.ToTopAssetWithHighestNegotiatedVolumeDto)
            .ToArray();

        return new GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult(assets);
    }
}