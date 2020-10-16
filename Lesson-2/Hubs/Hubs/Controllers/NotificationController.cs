using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Hubs.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hubs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {     
        public NotificationController()
        {

        }

        [HttpGet("[action]")]
        public async Task<ActionResult<string>> Test()
        {
            return "Hello";
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Push([FromBody]Message message, [FromServices]IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            await hubContext.Clients.All.Send(message);

            return Ok();
        }
    }
}
