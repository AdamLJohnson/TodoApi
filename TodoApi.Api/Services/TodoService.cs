using TodoApi.Api.Domain;
using TodoApi.Api.Repositories;

namespace TodoApi.Api.Services;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetTodos();
    Task<Todo> GetTodoById(Guid id);
    Task<Todo> CreateTodoAsync(Todo todo);
    Task<Todo> UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(Guid id);
    Task<bool> TodoExistsAsync(Guid id);
}

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<Todo>> GetTodos() => await _todoRepository.GetTodos();
    public async Task<Todo> GetTodoById(Guid id) => await _todoRepository.GetTodoById(id);
    public async Task<Todo> CreateTodoAsync(Todo todo) => await _todoRepository.CreateTodoAsync(todo);
    public async Task<Todo> UpdateTodoAsync(Todo todo) => await _todoRepository.UpdateTodoAsync(todo);
    public async Task DeleteTodoAsync(Guid id) => await _todoRepository.DeleteTodoAsync(id);
    public Task<bool> TodoExistsAsync(Guid id) => _todoRepository.TodoExistsAsync(id);
}