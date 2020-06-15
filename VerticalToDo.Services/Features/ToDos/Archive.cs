using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Abstractions.Exceptions;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Services.Features.ToDos
{
    public class Archive
    {
        public class Request : BaseRequest<Response>
        {
            public Guid Id { get; set; }
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

                item.IsArchived = true;

                Update(item);

                return new Response();
            }
        }
    }
}
