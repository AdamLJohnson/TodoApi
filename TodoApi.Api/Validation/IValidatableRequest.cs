using MediatR;

namespace TodoApi.Api.Validation;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequest { }

public interface IValidatableRequest { }