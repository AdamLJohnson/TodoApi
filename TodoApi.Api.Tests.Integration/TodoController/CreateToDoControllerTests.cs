using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using TodoApi.Api.Contracts.Requests;
using TodoApi.Api.Contracts.Responses;
using TodoApi.Api.Todos;
using TodoApi.Api.Validation;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController
{
    [Collection("TodoApiCollection")]
    public class CreateToDoControllerTests
    {
        private readonly HttpClient _client;
        private readonly Faker<CreateTodoRequest> _todoFaker = new Faker<CreateTodoRequest>()
            .RuleFor(t => t.Task, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence());

        public CreateToDoControllerTests(TodoApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateToDo_ReturnsCreatedToDo()
        {
            // Arrange
            var todo = _todoFaker.Generate();

            // Act
            var response = await _client.PostAsJsonAsync("/api/todo", todo);

            var responseContent = await response.Content.ReadAsStringAsync();
            var todoResponse = JsonConvert.DeserializeObject<TodoResponse>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().Be($"http://localhost/api/todo/{todoResponse.Id}");
            todoResponse.Should().BeEquivalentTo(todo);

            // Cleanup
            await _client.DeleteAsync($"api/todo/{todoResponse!.Id}");
        }

        [Fact]
        public async Task CreateToDo_ReturnsBadRequest_WhenTaskIsEmpty()
        {
            // Arrange
            var todo = _todoFaker.Generate();
            todo.Task = string.Empty;

            // Act
            var response = await _client.PostAsJsonAsync("/api/todo", todo);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Task").Which.Message.Should().Be("'Task' must not be empty.");
        }

        [Fact]
        public async Task CreateToDo_ReturnsBadRequest_WhenTaskIsNotUnique()
        {
            // Arrange
            var todo = _todoFaker.Generate();
            await _client.PostAsJsonAsync("/api/todo", todo);

            // Act
            var response = await _client.PostAsJsonAsync("/api/todo", todo);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Task").Which.Message.Should().Be("Task already exists");
        }

        [Fact]
        public async Task CreateToDo_ReturnsBadRequest_WhenDescriptionIsEmpty()
        {
            // Arrange
            var todo = _todoFaker.Generate();
            todo.Description = string.Empty;

            // Act
            var response = await _client.PostAsJsonAsync("/api/todo", todo);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ValidationFailureResponse>(responseContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse!.Errors.Should().Contain(c => c.PropertyName == "Description").Which.Message.Should().Be("'Description' must not be empty.");
        }
    }
}
