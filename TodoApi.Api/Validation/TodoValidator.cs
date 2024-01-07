using FluentValidation;
using TodoApi.Api.Domain;

namespace TodoApi.Api.Validation
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
