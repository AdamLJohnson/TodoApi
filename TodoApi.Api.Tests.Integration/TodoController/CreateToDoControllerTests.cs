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
using TodoApi.Api.Domain;
using Xunit;

namespace TodoApi.Api.Tests.Integration.TodoController
{
    [Collection("TodoApiCollection")]
    public class CreateToDoControllerTests
    {
        private readonly HttpClient _client;
        private readonly Faker<Todo> _todoFaker = new Faker<Todo>()
            .RuleFor(t => t.Id, f => f.Random.Guid())
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
            var todoResponse = await response.Content.ReadFromJsonAsync<Todo>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().Be($"http://localhost/api/todo/{todo.Id}");
            todoResponse.Should().BeEquivalentTo(todo);

            // Cleanup
            await _client.DeleteAsync($"api/todo/{todoResponse!.Id}");
        }
    }
}
