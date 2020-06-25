using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ServicesInstances.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : Controller
    {
        readonly IConfiguration _configuration;
        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //心跳,consul会每隔几秒调一次
            Console.WriteLine($"{ _configuration["port"]} Invoke");
            return Ok();
        }
    }
}