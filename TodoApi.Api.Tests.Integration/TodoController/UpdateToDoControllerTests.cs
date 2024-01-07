using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using TodoApi.Api.Domain;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

public class UpdateToDoControllerTests : IClassFixture<TodoApiFactory>
{
    private readonly HttpClient _client;
    private readonly Faker<Todo> _todoFaker = new Faker<Todo>()
        .RuleFor(t => t.Id, f => f.Random.Guid())
        .RuleFor(t => t.Task, f => f.Lorem.Word())
        .RuleFor(t => t.Description, f => f.Lorem.Sentence());

    public UpdateToDoControllerTests(TodoApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateToDo_ReturnsUpdatedTodo()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        var updatedTodo = _todoFaker.Generate();
        updatedTodo.Id = createdTodo!.Id;

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{createdTodo.Id}", updatedTodo);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var todosResponse = await response.Content.ReadFromJsonAsync<Todo>();
        todosResponse!.Should().BeEquivalentTo(updatedTodo);

        // Cleanup
        await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");
    }

    [Fact]
    public async Task UpdateToDo_ReturnsNotFound()
    {
        // Arrange
        var todo = _todoFaker.Generate();

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{todo.Id}", todo);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}