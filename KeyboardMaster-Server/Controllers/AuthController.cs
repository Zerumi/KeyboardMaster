// This code & software is licensed under the Creative Commons license. You can't use AMWE trademark 
// You can use & improve this code by keeping this comments
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using KeyboardMaster_Server.Hubs;
using KeyboardMaster_Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KeyboardMaster_Server.Controllers
{
    [Route("/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        public static uint GlobalClientId = 0;
        public static List<Client> GlobalUsersList = new List<Client>();

        [HttpPost]
        [AllowAnonymous]
        public async Task<dynamic> Auth([FromBody] string username)
        {
            var claims = new List<Claim>
                {
                new Claim(ClaimsIdentity.DefaultNameClaimType, $"ID {GlobalClientId}/" + username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Role.GlobalUserRole)
                };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            var client = new Client
            {
                Id = GlobalClientId,
                Username = username
            };
            GlobalUsersList.Add(client);
            GlobalClientId++;
            return client;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Logout(uint id)
        {
            var a = GlobalUsersList.Find(x => x.Id == id);
            (HttpContext.User.Identity as ClaimsIdentity).RemoveClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, Role.GlobalUserRole));
            GlobalUsersList.Remove(a);
            return NoContent();
        }
    }
}