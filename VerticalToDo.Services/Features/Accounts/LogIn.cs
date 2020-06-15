using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core.Features.Accounts;

namespace VerticalToDo.Services.Features.Accounts
{
    public class LogIn
    {
        public class Request : BaseRequest<Response>
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

        public class Response : AuthenticationResponse
        {

        }

        public class Validator : BaseValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).MinimumLength(6);
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await Query<Account>().FirstOrDefaultAsync(x => x.EmailAddress == request.EmailAddress);
                if (account == null || account.Password != request.Password)
                    throw new CustomException("Invalid email address or password");

                return new Response
                {
                    UserId = account.Id,
                    Username = account.EmailAddress
                };
            }
        }
    }
}
