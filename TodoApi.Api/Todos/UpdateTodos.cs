using FluentValidation;
using MediatR;
using TodoApi.Api.Validation;

namespace TodoApi.Api.Todos;

public record UpdateTodoCommand
    (Guid Id, string Task, string Description, bool IsComplete) : IValidatableRequest<Result<Todo, ValidationFailed>>;

#pragma warning disable CS8631
public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Result<Todo?, ValidationFailed>>
#pragma warning restore CS8631
{
    private readonly ITodoRepository _repository;

    public UpdateTodoCommandHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Todo?, ValidationFailed>> Handle(UpdateTodoCommand command,
        CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = command.Id,
            Task = command.Task,
            Description = command.Description,
            IsComplete = command.IsComplete
        };

        var exists = await _repository.ExistsAsync(todo.Id);
        if (!exists)
        {
            return default(Todo?);
        }

        await _repository.UpdateAsync(todo);
        return todo;
    }
}

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    private readonly ITodoRepository _repository;

    public UpdateTodoCommandValidator(ITodoRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Task).NotEmpty();
        RuleFor(x => x).MustAsync(IsUniqueAsync).WithName("Task").WithMessage("Task already exists");
        RuleFor(x => x.Description).NotEmpty();
    }

    private async Task<bool> IsUniqueAsync(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetAsync(command.Task);
        if (todo == null)
        {
            return true;
        }
        return todo.Id == command.Id;
    }
}