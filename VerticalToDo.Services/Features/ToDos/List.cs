using System.Threading;
using System.Threading.Tasks;

namespace VerticalToDo.Services.Features.ToDos
{
    public class List
    {
        public class Request : BasePaginationRequest<Response>
        {

        }

        public class Response : BasePaginationResponse
        {

        }

        public class Validator : BasePaginationValidator<Request, Response>
        {
            public Validator() : base()
            {

            }
        }

        public class Handler : BasePaginationHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new Response());
            }
        }
    }
}
