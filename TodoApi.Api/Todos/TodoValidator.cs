using FluentValidation;

namespace TodoApi.Api.Todos
{
    public class TodoValidator : AbstractValidator<Todo>
    {
        public TodoValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Task).NotEmpty();
            RuleFor(t => t.Description).NotEmpty();
        }
    }
}
