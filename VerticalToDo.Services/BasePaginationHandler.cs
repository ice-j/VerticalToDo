namespace VerticalToDo.Services
{
    public abstract class BasePaginationHandler<TRequest, TResponse> : BaseHandler<TRequest, TResponse>
			 where TRequest : BasePaginationRequest<TResponse>
			 where TResponse : BasePaginationResponse
	{
    }
}
