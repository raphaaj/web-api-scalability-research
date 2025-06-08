using System.ComponentModel.DataAnnotations;
using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;
using B3.QuotationHistories.WebApi.Models.Assets;
using Microsoft.AspNetCore.Mvc;

namespace B3.QuotationHistories.WebApi.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController(
    GetTopNAssetsWithHighestNegotiatedVolumeUseCase getTopNAssetsWithHighestNegotiatedVolumeUseCase)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTopNAssetsWithHighestNegotiatedVolumeAsync(
        [FromQuery,
         Range(GetTopNAssetsWithHighestNegotiatedVolumeUseCase.MinNumberOfTopAssetsWithHighestNegotiatedVolume,
             GetTopNAssetsWithHighestNegotiatedVolumeUseCase.MaxNumberOfTopAssetsWithHighestNegotiatedVolume)]
        int n)
    {
        var getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest =
            new GetTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest
            {
                NumberOfTopAssetsWithHighestNegotiatedVolume = n
            };

        var getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult =
            await getTopNAssetsWithHighestNegotiatedVolumeUseCase.ExecuteAsync(
                getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandRequest);

        var getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResponse =
            new GetTopNAssetsWithHighestNegotiatedVolumeResponse(
                getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResult.Assets
                    .Select(x =>
                        new TopAssetWithHighestNegotiatedVolumeResponse(x.PaperNegotiationCode,
                            x.TotalVolumeOfTilesNegotiated)
                    )
                    .ToArray());

        return Ok(getTopNAssetsWithHighestNegotiatedVolumeUseCaseCommandResponse);
    }
}