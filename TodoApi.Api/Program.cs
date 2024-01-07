using FluentValidation;
using TodoApi.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using TodoApi.Api.Validation;
using TodoApi.Api.Todos;
using TodoApi.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("TodoDb"));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.TryAddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining<Program>();
    c.AddValidation<CreateTodoCommand, Todo>();
    c.AddValidation<UpdateTodoCommand, Todo?>();
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.Run();
