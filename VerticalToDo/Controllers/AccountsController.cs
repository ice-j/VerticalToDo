using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VerticalToDo.Services.Features.Accounts;

namespace VerticalToDo.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : BaseController
    {
        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<LogIn.Response> LogIn([FromBody] LogIn.Request request)
            => await Handle(request);

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<Register.Response> Register([FromBody] Register.Request request)
            => await Handle(request);
    }
}
