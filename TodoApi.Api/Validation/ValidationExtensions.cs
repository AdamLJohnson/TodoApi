using MediatR;

namespace TodoApi.Api.Validation;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull, IRequest<Result<TResponse, ValidationFailed>>
    {
        return config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse, ValidationFailed>>, ValidationBehavior<TRequest, TResponse>>();
    }
}