using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace servicedesk.api
{
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        public ProfileController()
        {
        }

        [HttpGet, Authorize]
        public IActionResult Get()
        {
            var identity = (ClaimsIdentity)User.Identity;

            if (identity == null)
            {
                throw new Exception("Profile: identity is null");
            }
            if (identity.Claims == null)
            {
                throw new Exception("Profile: identity.Claims is null");
            }

            var roles = identity.Claims
                .Where(r => r.Type == "role")
                .Select(r => r.Value)
                .ToList();

            var name = identity.Claims
                .Where(r => r.Type == "name")
                .Select(r => r.Value)
                .SingleOrDefault();

            var id = identity.Claims
                .Where(r => r.Type == "sub")
                .Select(r => r.Value)
                .SingleOrDefault();

            var result = new
            {
                id,
                name,
                roles
            };

            return Ok(result);
        }
    }
}
