using MediatR;

namespace VerticalToDo.Services
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse>
        where TResponse : BaseResponse
    {
    }
}
