using System;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Services.Features.ToDos
{
    public class Update
    {
        public class Request : BaseRequest<Response>
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTimeOffset? DueDate { get; set; }
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
                var item = await GetById<ToDo>(request.Id);
                if (item == null)
                    throw new CustomException("Item not found");

                if (item.AccountId != _currentUser.Id)
                    throw new CustomException("You can only edit your own ToDo items");

                item.Title = request.Title;
                item.Description = request.Description;
                item.DueDate = request.DueDate;

                Update(item);

                return new Response();
            }
        }
    }
}
