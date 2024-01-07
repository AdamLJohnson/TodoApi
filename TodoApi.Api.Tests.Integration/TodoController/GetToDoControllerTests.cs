using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using TodoApi.Api.Contracts.Requests;
using TodoApi.Api.Contracts.Responses;
using TodoApi.Api.Todos;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

[Collection("TodoApiCollection")]
public class GetToDoControllerTests
{
    private readonly HttpClient _client;
    private readonly Faker<CreateTodoRequest> _todoFaker = new Faker<CreateTodoRequest>()
        .RuleFor(t => t.Task, f => f.Lorem.Word())
        .RuleFor(t => t.Description, f => f.Lorem.Sentence());

    public GetToDoControllerTests(TodoApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetToDo_ReturnsSingleTodo()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoResponse>();

        // Act
        var response = await _client.GetAsync($"api/todo/{createdTodo!.Id}");

        var responseContent = await response.Content.ReadAsStringAsync();
        var todoResponse = JsonConvert.DeserializeObject<TodoResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        todoResponse!.Should().BeEquivalentTo(createdTodo);

        // Cleanup
        await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");
    }

    [Fact]
    public async Task GetToDo_ReturnsNotFound()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"api/todo/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}