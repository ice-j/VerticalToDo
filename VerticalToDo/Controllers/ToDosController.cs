using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VerticalToDo.Services.Features.ToDos;

namespace VerticalToDo.Controllers
{
    [Route("api/[controller]")]
    public class ToDosController : BaseController
    {
        [HttpGet]
        [Route("[action]")]
        public async Task<List.Response> List([FromQuery] List.Request request)
            => await Handle(request);

        [HttpPost]
        [Route("[action]")]
        public async Task<New.Response> New([FromBody] New.Request request)
            => await Handle(request);

        [HttpPost]
        [Route("[action]")]
        public async Task<Update.Response> Update([FromBody] Update.Request request)
            => await Handle(request);
    }
}
