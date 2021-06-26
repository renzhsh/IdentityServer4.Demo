using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(1);

            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
