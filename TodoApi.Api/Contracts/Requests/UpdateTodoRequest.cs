namespace TodoApi.Api.Contracts.Requests;

public class UpdateTodoRequest
{
    public string Task { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}