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
public class UpdateToDoControllerTests
{
    private readonly HttpClient _client;
    private readonly Faker<UpdateTodoRequest> _todoFaker = new Faker<UpdateTodoRequest>()
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
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoResponse>();

        var updatedTodo = _todoFaker.Generate();

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{createdTodo.Id}", updatedTodo);

        var responseContent = await response.Content.ReadAsStringAsync();
        var todoResponse = JsonConvert.DeserializeObject<TodoResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        todoResponse!.Should().BeEquivalentTo(updatedTodo);

        // Cleanup
        await _client.DeleteAsync($"/api/todo/{createdTodo!.Id}");
    }

    [Fact]
    public async Task UpdateToDo_ReturnsNotFound()
    {
        // Arrange
        var todo = _todoFaker.Generate();

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{new Guid()}", todo);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateToDo_ReturnsBadRequest_WhenTaskIsEmpty()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        todo.Task = string.Empty;

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{Guid.NewGuid()}", todo);

        var responseContent = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Task").Which.Message.Should().Be("'Task' must not be empty.");
    }

    [Fact]
    public async Task UpdateToDo_ReturnsBadRequest_WhenTaskIsNotUnique()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        var todoResponse = await _client.PostAsJsonAsync("/api/todo", todo);
        var createdTodo = await todoResponse.Content.ReadFromJsonAsync<TodoResponse>();

        var todo2 = _todoFaker.Generate();
        await _client.PostAsJsonAsync("/api/todo", todo2);

        todo.Task = todo2.Task;

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{createdTodo.Id}", todo);

        var responseContent = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Task").Which.Message.Should().Be("Task already exists");
    }

    [Fact]
    public async Task UpdateToDo_ReturnsBadRequest_WhenDescriptionIsEmpty()
    {
        // Arrange
        var todo = _todoFaker.Generate();
        todo.Description = string.Empty;

        // Act
        var response = await _client.PutAsJsonAsync($"api/todo/{Guid.NewGuid()}", todo);

        var responseContent = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Description").Which.Message.Should().Be("'Description' must not be empty.");
    }
}