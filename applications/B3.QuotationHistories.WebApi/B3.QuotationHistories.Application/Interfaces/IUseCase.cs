namespace B3.QuotationHistories.Application.Interfaces;

public interface IUseCase<TCommandRequest, TCommandResponse>
    where TCommandRequest : IUseCaseCommandRequest
    where TCommandResponse : IUseCaseCommandResult
{
    Task<TCommandResponse> ExecuteAsync(TCommandRequest commandRequest);
}