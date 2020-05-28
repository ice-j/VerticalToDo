using MediatR;

namespace VerticalToDo.Services
{
    public class BaseValidator<TRequest> : FluentValidation.AbstractValidator<TRequest>
            where TRequest : IRequest<object>
    {
    }
}
