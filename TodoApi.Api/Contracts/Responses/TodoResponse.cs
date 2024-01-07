namespace TodoApi.Api.Contracts.Responses;

public class TodoResponse
{
    public Guid Id { get; set; }
    public string Task { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}