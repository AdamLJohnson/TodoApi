namespace TodoApi.Api.Contracts.Responses;

public class TodosResponse
{
    public required IEnumerable<TodoResponse> Items { get; init; } = Enumerable.Empty<TodoResponse>();
}