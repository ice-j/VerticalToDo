using System;
using System.Threading;
using System.Threading.Tasks;

namespace VerticalToDo.Services.Features.ToDos
{
    public class Update
    {
        public class Request : BaseRequest<Response>
        {
            public Guid Id { get; set; }
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
