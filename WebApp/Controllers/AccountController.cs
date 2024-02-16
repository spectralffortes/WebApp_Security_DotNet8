using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Data.Account;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> LoginExternallyCallback(string returnUrl)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("External");

            if (!authenticateResult.Succeeded)
                return BadRequest(); // TODO: Handle this better.

            var claimsIdentity = new ClaimsIdentity("Application");

            claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier));
            claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Email));

            await HttpContext.SignInAsync(
                "Application", 
                new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(returnUrl);
        }

    }
}
