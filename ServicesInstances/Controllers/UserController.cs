using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;

namespace ServicesInstances.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        readonly IUserInfo _userInfo;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserInfo userInfo,
            ILogger<UserController> logger)
        {
            _userInfo = userInfo;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize]
        public async Task<CustomerUser> GetCustomerUser()
        {
            _logger.LogInformation(DateTime.Now + "调用了一次");
            return await _userInfo.QueryUserInfoById(1);
        }

        [HttpGet]
        public async Task<CustomerUser> TimeOut()
        {
            Thread.Sleep(5000);
            _logger.LogInformation(DateTime.Now + "调用了一次");
            return await _userInfo.QueryUserInfoById(1);
        }
    }
}