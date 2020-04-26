using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EM.Sample.WebApi.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthContextController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var userId = this.User.FindFirstValue(JwtClaimTypes.Subject);
            UserInfo authContext = new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserId = userId,
                Name = User.Identity.Name,
                Claims = User.Claims.Select(i => new SimpleClaim { Type = i.Type, Value = i.Value }).ToList()
            };
            
            return Ok(authContext);
        }
    }
}