using System.Threading;
using System.Threading.Tasks;

namespace VerticalToDo.Services.Features.ToDos
{
    public class New
    {
        public class Request : BaseRequest<Response>
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class Response : BaseResponse
        {

        }

        public class Validator : BaseValidator<Request>
        {
            public Validator()
            {

            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new Response());
            }
        }
    }
}
