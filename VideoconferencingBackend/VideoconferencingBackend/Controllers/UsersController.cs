using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VideoconferencingBackend.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class UsersController : Controller
    {
        [HttpGet]
        [Route("kek")]
        public IActionResult Keke()
        {
            return new OkObjectResult("Hi");
        }
    }
}