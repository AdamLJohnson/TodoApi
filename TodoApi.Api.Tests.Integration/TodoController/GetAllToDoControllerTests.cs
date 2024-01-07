using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using TodoApi.Api.Contracts.Requests;
using TodoApi.Api.Contracts.Responses;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController;

[Collection("TodoApiCollection")]
public class GetAllToDoControllerTests
{
    private readonly HttpClient _client;
    private readonly Faker<CreateTodoRequest> _todoFaker = new Faker<CreateTodoRequest>()
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
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoResponse>();

        // Act
        var response = await _client.GetAsync($"/api/todo");
        var responseContent = await response.Content.ReadAsStringAsync();
        var todosResponse = JsonConvert.DeserializeObject<TodosResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        todosResponse!.Items.Should().HaveCountGreaterThan(0);
        todosResponse!.Items.Should().ContainEquivalentOf(createdTodo);

        // Cleanup
        await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");
    }
}