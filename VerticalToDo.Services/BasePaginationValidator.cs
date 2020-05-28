using FluentValidation;

namespace VerticalToDo.Services
{
    public class BasePaginationValidator<TRequest, TResponse> : BaseValidator<TRequest>
			where TRequest : BasePaginationRequest<TResponse>
			where TResponse : BasePaginationResponse
	{
		public BasePaginationValidator()
		{
			RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
			RuleFor(x => x.PageSize).GreaterThan(0);
		}
    }
}
