using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using TodoApi.Api.Domain;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

public class DeleteToDoControllerTests : IClassFixture<TodoApiFactory>
{
    private readonly HttpClient _client;
    private readonly Faker<Todo> _todoFaker = new Faker<Todo>()
        .RuleFor(t => t.Id, f => f.Random.Guid())
        .RuleFor(t => t.Task, f => f.Lorem.Word())
        .RuleFor(t => t.Description, f => f.Lorem.Sentence());

    public DeleteToDoControllerTests(TodoApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteToDo_ReturnsNoContent()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Act
        var response = await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}