using FluentValidation.Results;
using TodoApi.Api.Contracts.Responses;
using TodoApi.Api.Todos;
using TodoApi.Api.Validation;

namespace TodoApi.Api.Contracts;

public static class Mapping
{
    public static TodoResponse MapToResponse(this Todo todo)
    {
        return new TodoResponse
        {
            Id = todo.Id,
            Task = todo.Task,
            Description = todo.Description,
            IsComplete = todo.IsComplete
        };
    }

    public static TodosResponse MapToResponse(this IEnumerable<Todo> todos)
    {
        return new TodosResponse
        {
            Items = todos.Select(x => new TodoResponse
            {
                Id = x.Id,
                Task = x.Task,
                Description = x.Description,
                IsComplete = x.IsComplete
            })
        };
    }

    public static ValidationFailureResponse MapToResponse(this IEnumerable<ValidationFailure> validationFailures)
    {
        return new ValidationFailureResponse
        {
            Errors = validationFailures.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            })
        };
    }

    public static ValidationFailureResponse MapToResponse(this ValidationFailed failed)
    {
        return new ValidationFailureResponse
        {
            Errors = failed.Errors.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            })
        };
    }
}