using FluentValidation;
using TodoApi.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using TodoApi.Api.Validation;
using TodoApi.Api.Todos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("TodoDb"));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.TryAddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining<Program>();
    c.AddValidation<CreateTodoCommand, Todo>();
#pragma warning disable CS8631
    c.AddValidation<UpdateTodoCommand, Todo?>();
#pragma warning restore CS8631
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI((options =>
{
    options.RoutePrefix = "";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API");
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.Run();
