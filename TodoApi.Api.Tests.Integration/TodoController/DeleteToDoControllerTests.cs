using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using TodoApi.Api.Contracts.Requests;
using TodoApi.Api.Contracts.Responses;
using TodoApi.Api.Todos;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

[Collection("TodoApiCollection")]
public class DeleteToDoControllerTests
{
    private readonly HttpClient _client;
    private readonly Faker<CreateTodoRequest> _todoFaker = new Faker<CreateTodoRequest>()
        .RuleFor(t => t.Task, f => f.Lorem.Word())
        .RuleFor(t => t.Description, f => f.Lorem.Sentence());

    public DeleteToDoControllerTests(TodoApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteToDo_ReturnsOk()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteToDo_ReturnsNotFound()
    {
        // Arrange

        // Act
        var response = await _client.DeleteAsync($"/api/todo/{new Guid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}