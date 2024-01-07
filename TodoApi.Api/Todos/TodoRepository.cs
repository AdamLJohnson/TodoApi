using Microsoft.EntityFrameworkCore;
using TodoApi.Api.Database;

namespace TodoApi.Api.Todos;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetTodos();
    Task<Todo> GetTodoById(Guid id);
    Task<Todo> CreateTodoAsync(Todo todo);
    Task<Todo> UpdateTodoAsync(Todo todo);
    Task<bool> DeleteTodoAsync(Guid id);
    Task<bool> TodoExistsAsync(Guid id);
    Task<bool> TodoExistsAsync(string task);
    Task<Todo?> GetToDoByTask(string task);
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

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return false;
        }
        _context.Todos.Remove(todo);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> TodoExistsAsync(Guid id) => await _context.Todos.AnyAsync(e => e.Id == id);
    public async Task<bool> TodoExistsAsync(string task) => await _context.Todos.AnyAsync(e => e.Task == task);
    public async Task<Todo?> GetToDoByTask(string task) => await _context.Todos.FirstOrDefaultAsync(e => e.Task == task);
}