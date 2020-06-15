using System;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Services.Features.ToDos
{
    public class Details
    {
        public class Request : BaseRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : BaseResponse
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTimeOffset? DueDate { get; set; }
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
                var item = await GetById<ToDo>(request.Id);
                if (item == null)
                    throw new CustomException("ToDo item not found");

                if (item.AccountId != _currentUser.Id)
                    throw new CustomException("You can only see your own todos");

                return new Response
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    DueDate = item.DueDate
                };
            }
        }
    }
}
