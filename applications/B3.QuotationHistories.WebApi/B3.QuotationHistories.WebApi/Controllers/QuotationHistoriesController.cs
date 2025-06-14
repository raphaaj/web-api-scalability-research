using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.Domain.Exceptions;
using B3.QuotationHistories.WebApi.Mappers;
using B3.QuotationHistories.WebApi.Models.QuotationHistories;
using Microsoft.AspNetCore.Mvc;

namespace B3.QuotationHistories.WebApi.Controllers;

[ApiController]
[Route("api/quotation-histories")]
public class QuotationHistoriesController(GetPaperQuotationHistoriesUseCase getPaperQuotationHistoriesUseCase)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPaperQuotationHistoriesAsync(
        [FromQuery] string paperNegotiationCode)
    {
        var getPaperHistoricalQuotationsCommandRequest = new GetPaperQuotationHistoriesUseCaseCommandRequest
        {
            PaperNegotiationCode = paperNegotiationCode,
        };

        GetPaperQuotationHistoriesUseCaseCommandResult getPaperHistoricalQuotationsCommandResult;
        try
        {
            getPaperHistoricalQuotationsCommandResult =
                await getPaperQuotationHistoriesUseCase.ExecuteAsync(getPaperHistoricalQuotationsCommandRequest);
        }
        catch (InvalidPaperNegotiationCodeException invalidPaperNegotiationCodeException)
        {
            ModelState.AddModelError(nameof(paperNegotiationCode),
                invalidPaperNegotiationCodeException.Message);
            return ValidationProblem(ModelState);
        }

        var quotationHistories = getPaperHistoricalQuotationsCommandResult.QuotationHistories
            .Select(PaperQuotationHistoryDtoMapper.ToGetPaperHistoricalQuotationResponse)
            .ToArray();

        var getPaperHistoricalQuotationsResponse = new GetPaperQuotationHistoriesResponse(quotationHistories);

        return Ok(getPaperHistoricalQuotationsResponse);
    }
}