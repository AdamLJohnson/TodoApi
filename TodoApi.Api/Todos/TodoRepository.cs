using Microsoft.EntityFrameworkCore;
using TodoApi.Api.Database;

namespace TodoApi.Api.Todos;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetAsync(Guid id);
    Task<Todo?> GetAsync(string task);
    Task<Todo> CreateAsync(Todo todo);
    Task<Todo> UpdateAsync(Todo todo);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string task);
}

public class TodoRepository : ITodoRepository
{
    private readonly DatabaseContext _context;

    public TodoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync() => await _context.Todos.ToListAsync();
    public async Task<Todo?> GetAsync(Guid id) => await _context.Todos.FindAsync(id);

    public async Task<Todo> CreateAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> UpdateAsync(Todo todo)
    {
        _context.Entry(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<bool> DeleteAsync(Guid id)
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

    public async Task<bool> ExistsAsync(Guid id) => await _context.Todos.AnyAsync(e => e.Id == id);
    public async Task<bool> ExistsAsync(string task) => await _context.Todos.AnyAsync(e => e.Task == task);
    public async Task<Todo?> GetAsync(string task) => await _context.Todos.FirstOrDefaultAsync(e => e.Task == task);
}