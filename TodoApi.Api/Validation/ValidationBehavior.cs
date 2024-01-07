using FluentValidation;
using MediatR;

namespace TodoApi.Api.Validation;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult, ValidationFailed>> where TRequest : notnull, IRequest<Result<TResult, ValidationFailed>>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<Result<TResult, ValidationFailed>> Handle(
        TRequest request,
        RequestHandlerDelegate<Result<TResult, ValidationFailed>> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        return await next();
    }
}