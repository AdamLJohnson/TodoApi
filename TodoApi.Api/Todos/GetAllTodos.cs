using MediatR;

namespace TodoApi.Api.Todos;

public record GetAllTodosQuery : IRequest<IEnumerable<Todo>>;

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, IEnumerable<Todo>>
{
    private readonly ITodoRepository _repository;

    public GetAllTodosQueryHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Todo>> Handle(GetAllTodosQuery query, CancellationToken cancellationToken)
    {
        var todos = await _repository.GetAllAsync();
        return todos;
    }
}