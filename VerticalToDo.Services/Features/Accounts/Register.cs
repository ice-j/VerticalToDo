using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace VerticalToDo.Services.Features.Accounts
{
    public class Register
    {
        public class Request : BaseRequest<Response>
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class Response : AuthenticationResponse
        {

        }

        public class Validator : BaseValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Password).Matches(x => x.ConfirmPassword).WithMessage("Password and ConfirmPassword fields doesn't match.");
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
