using Lamar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VerticalToDo.Services;

namespace VerticalToDo
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        [SetterProperty]
        public IMediator Mediator
        {
            set
            {
                if (_mediator != null)
                    throw new InvalidOperationException("Cannot change Mediator property! It should be dependency resolved!");
                else
                    _mediator = value;
            }
        }

        protected async Task<TResponse> Handle<TResponse>(BaseRequest<TResponse> request)
            where TResponse : BaseResponse
        {
            return await _mediator.Send(request);
        }
    }
}
