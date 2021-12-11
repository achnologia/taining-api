using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace training_api.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/test")]
        public IActionResult Main()
        {
            return Ok(new { name = "test" });
        }
    }
}
