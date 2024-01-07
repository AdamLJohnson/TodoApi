using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Api.Contracts;
using TodoApi.Api.Contracts.Requests;
using TodoApi.Api.Contracts.Responses;
using TodoApi.Api.Database;

namespace TodoApi.Api.Todos
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllTodosQuery();
            var result = await _mediator.Send(query);
            return Ok(result.MapToResponse());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetTodoQuery(id);
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result.MapToResponse()) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTodoRequest request)
        {
            var command = new UpdateTodoCommand(id, request.Task, request.Description, request.IsComplete);
            var result = await _mediator.Send(command);
            return result.Match<IActionResult>(
                t => t is not null ? Ok(t.MapToResponse()) : NotFound(),
                failed => BadRequest(failed.MapToResponse()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoRequest request)
        {
            var command = new CreateTodoCommand(request.Task, request.Description, request.IsComplete);
            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                t => CreatedAtAction(nameof(Get), new { id = t.Id }, t.MapToResponse()),
                failed => BadRequest(failed.MapToResponse()));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteTodoCommand(id);
            var result = await _mediator.Send(command);
            return result ? Ok() : NotFound();
        }
    }
}
