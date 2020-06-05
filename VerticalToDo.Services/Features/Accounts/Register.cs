using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core.Features.Accounts;

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
                RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).MinimumLength(6).Matches(x => x.ConfirmPassword).WithMessage("Password and ConfirmPassword fields doesn't match.");
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await Query<Account>().FirstOrDefaultAsync(x => x.EmailAddress == request.EmailAddress);
                if (account != null)
                    throw new CustomException("An account with the same email address already exists");

                account = new Account()
                {
                    EmailAddress = request.EmailAddress,
                    Password = request.Password
                };

                var id = Add(account);
                return new Response()
                {
                    UserId = id,
                    Username = account.EmailAddress
                };
            }
        }
    }
}
