namespace TodoApi.Api.Contracts.Responses;

public class TodoResponse
{
    public Guid Id { get; set; }
    public string Task { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}