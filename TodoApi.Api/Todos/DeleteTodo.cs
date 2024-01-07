using MediatR;

namespace TodoApi.Api.Todos;

public record DeleteTodoCommand(Guid Id) : IRequest<bool>;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
{
    private readonly ITodoRepository _repository;

    public DeleteTodoCommandHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(command.Id);
        return result;
    }
}