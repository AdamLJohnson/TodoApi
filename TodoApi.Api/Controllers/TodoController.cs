using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Api.Database;
using TodoApi.Api.Domain;
using TodoApi.Api.Services;

namespace TodoApi.Api.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var output = await _service.GetTodos();
            return output.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
        {
            var todo = await _service.GetTodoById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> PutTodo(Guid id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateTodoAsync(todo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _service.TodoExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _service.CreateTodoAsync(todo);

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _service.GetTodoById(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _service.DeleteTodoAsync(id);

            return NoContent();
        }
    }
}
