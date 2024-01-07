using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using TodoApi.Api.Domain;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

[Collection("TodoApiCollection")]
public class GetAllToDoControllerTests
{
    private readonly HttpClient _client;
    private readonly Faker<Todo> _todoFaker = new Faker<Todo>()
        .RuleFor(t => t.Id, f => f.Random.Guid())
        .RuleFor(t => t.Task, f => f.Lorem.Word())
        .RuleFor(t => t.Description, f => f.Lorem.Sentence());

    public GetAllToDoControllerTests(TodoApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllToDo_ReturnsListOfTodos()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<Todo>();

        // Act
        var response = await _client.GetAsync($"/api/todo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var todosResponse = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>();
        todosResponse!.Single().Should().BeEquivalentTo(createdTodo);

        // Cleanup
        await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");
    }
}