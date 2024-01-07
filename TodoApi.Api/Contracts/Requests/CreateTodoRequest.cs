namespace TodoApi.Api.Contracts.Requests;

public class CreateTodoRequest
{
    public string Task { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}