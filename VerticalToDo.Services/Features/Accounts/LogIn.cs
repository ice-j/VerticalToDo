using System.Threading;
using System.Threading.Tasks;

namespace VerticalToDo.Services.Features.Accounts
{
    public class LogIn
    {
        public class Request : BaseRequest<Response>
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
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
