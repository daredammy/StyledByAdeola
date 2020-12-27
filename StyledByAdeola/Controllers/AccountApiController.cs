using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
//using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging.ApplicationInsights;
using StyledByAdeola.Models;
using StyledByAdeola.Models.ViewModels;
using Microsoft.Azure.KeyVault.Models;

namespace StyledByAdeola.Controllers {
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class AccountApiController : Controller {
        // if there is an authentication failure, then I create a model validation error and render the default view; 
        //however, if authentication is successful, then I redirect the user to the URL
        private UserManager<AppUser> userManager;
        //private IUserValidator<AppUser> userValidator;
        //private IPasswordValidator<AppUser> passwordValidator;
        //private IPasswordHasher<AppUser> passwordHasher;
        private SignInManager<AppUser> signInManager;

        public AccountApiController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        [HttpPost("/api/account/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel creds) {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(creds.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                            creds.Password, false, false)).Succeeded)
                    {
                        return Ok("true");
                    }
                }
                return BadRequest("Invalid credentials");
            }
            return BadRequest();
        }        
        
        [HttpPost("/api/account/signup")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SignUp([FromBody] LoginModel creds) {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(creds.Email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = creds.Email,
                        UserName = creds.Email
                    };
                    IdentityResult identResult = await userManager.CreateAsync(user);
                    if (identResult.Succeeded)
                    {
                        return Ok("true");
                    }
                }
                else
                {
                    return Ok("User exists in db");
                }
            }
            return BadRequest();
        }

        [HttpPost("/api/account/logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Login using external identity provider, 
        /// </summary>
        /// <param name="loginModel">User login model</param>
        /// <returns></returns>
        [HttpPost("/api/account/externalidentitylogin")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult ExternalIdentityLogin([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid) { 
                if (loginModel.IdentityProvider == "Microsoft" || loginModel.IdentityProvider == "Google")
                {
                    string redirectUrl = Url.Action("ExternalIdentityProviderResponse", "Account", new { loginModel.ReturnUrl });
                    var properties = signInManager.ConfigureExternalAuthenticationProperties(loginModel.IdentityProvider, redirectUrl);
                    return new ChallengeResult(loginModel.IdentityProvider, properties);
                }
                else
                {
                    throw new System.InvalidOperationException("External Identity Provider is not configured");
                }
            }
            return BadRequest();
    }

        /// <summary>
        /// Get response from idauth external identity provider, 
        /// save user info if not already in Db
        /// </summary>
        /// <param name="returnUrl">ReturnUrl from brower</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> ExternalIdentityProviderResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest();
            }
            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Ok("true");
            }
            else
            {
                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName =
                        info.Principal.FindFirst(ClaimTypes.Email).Value
                };
                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return Ok("true");
                    }
                }
                return BadRequest();
            }
        }
    }
}
