using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Services.Features.ToDos
{
    public class ListArchived
    {
        public class Request : BasePaginationRequest<Response>
        {

        }

        public class Response : BasePaginationResponse
        {
            public List<Item> Items { get; set; }
            public class Item
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public string DueDate { get; set; }
            }
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
                var items = await Query<ToDo>().Where(x => x.AccountId == _currentUser.Id && x.IsArchived).Take(request.PageSize).Skip(request.Skip).ToListAsync();
                return new Response
                {
                    Items = items.Select(x => new Response.Item
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        DueDate = x.DueDate?.Date.ToShortDateString()
                    }).ToList()
                };
            }
        }
    }
}
