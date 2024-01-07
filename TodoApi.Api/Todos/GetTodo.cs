using MediatR;

namespace TodoApi.Api.Todos;

public record GetTodoQuery(Guid Id) : IRequest<Todo?>;

public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, Todo?>
{
    private readonly ITodoRepository _repository;

    public GetTodoQueryHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Todo?> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetTodoById(query.Id);
        return todo;
    }
}