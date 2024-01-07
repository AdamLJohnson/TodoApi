using Microsoft.EntityFrameworkCore;
using TodoApi.Api.Database;
using TodoApi.Api.Domain;

namespace TodoApi.Api.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetTodos();
    Task<Todo> GetTodoById(Guid id);
    Task<Todo> CreateTodoAsync(Todo todo);
    Task<Todo> UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(Guid id);
    Task<bool> TodoExistsAsync(Guid id);
}

public class TodoRepository : ITodoRepository
{
    private readonly DatabaseContext _context;

    public TodoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> GetTodos() => await _context.Todos.ToListAsync();
    public async Task<Todo> GetTodoById(Guid id) => await _context.Todos.FindAsync(id);
    
    public async Task<Todo> CreateTodoAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> UpdateTodoAsync(Todo todo)
    {
        _context.Entry(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task DeleteTodoAsync(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TodoExistsAsync(Guid id) => await _context.Todos.AnyAsync(e => e.Id == id);
}