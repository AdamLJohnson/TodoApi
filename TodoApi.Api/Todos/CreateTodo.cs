using FluentValidation;
using MediatR;
using TodoApi.Api.Validation;

namespace TodoApi.Api.Todos;

public record CreateTodoCommand
    (string Task, string Description, bool IsComplete) : IValidatableRequest<Result<Todo, ValidationFailed>>;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Result<Todo, ValidationFailed>>
{
    private readonly ITodoRepository _repository;

    public CreateTodoCommandHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Todo, ValidationFailed>> Handle(CreateTodoCommand command,
        CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Task = command.Task,
            Description = command.Description,
            IsComplete = command.IsComplete
        };

        await _repository.CreateAsync(todo);
        return todo;
    }
}

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    private readonly ITodoRepository _repository;

    public CreateTodoCommandValidator(ITodoRepository repository)
    {
        _repository = repository;
        RuleFor(x => x.Task).NotEmpty();
        RuleFor(x => x.Task).MustAsync(IsUniqueAsync).WithMessage("Task already exists");
        RuleFor(x => x.Description).NotEmpty();
    }

    private async Task<bool> IsUniqueAsync(CreateTodoCommand command, string task, CancellationToken cancellationToken)
    {
        var todo = await _repository.ExistsAsync(task);
        return !todo;
    }
}