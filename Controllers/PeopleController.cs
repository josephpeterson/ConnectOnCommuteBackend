using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectOnCommuteBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectOnCommuteBackend.Controllers
{
    public class PeopleController : Controller
    {
        [HttpPost]
        [Route("api/people")]
        public IActionResult SendUserLocation([FromBody] UserLocation location)
        {
            return new OkObjectResult("");
        }

        // GET api/values/5
        [HttpGet]
        [Route("api/people")]

        public IActionResult Get()
        {
            return new OkObjectResult("test");
        }
    }
}
