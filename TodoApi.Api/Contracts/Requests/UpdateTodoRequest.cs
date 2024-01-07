namespace TodoApi.Api.Contracts.Requests;

public class UpdateTodoRequest
{
    public string Task { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}