using System;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Services.Features.ToDos
{
    public class New
    {
        public class Request : BaseRequest<Response>
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTimeOffset? DueDate { get; set; }
        }

        public class Response : BaseResponse
        {
            public Guid Id { get; set; }
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
                var item = new ToDo
                {
                    Title = request.Title,
                    AccountId = _currentUser.Id,
                    Description = request.Description,
                    DueDate = request.DueDate
                };
                var id = Add(item);
                return new Response
                {
                    Id = id
                };
            }
        }
    }
}
