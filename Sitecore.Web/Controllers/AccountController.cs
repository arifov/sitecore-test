using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sitecore.Membership;
using Sitecore.Web.Common;
using Sitecore.Web.Models;

namespace Sitecore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager _userManager = new UserManager(@"https://localhost:44320/api/");

        [HttpPost("token")]
        public ActionResult Token()
        {
            var email = Request.Form["email"];
            var password = Request.Form["password"];

            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                var errors = new List<string>() { "Invalid username or password." };
                return GetErrorResult(new ErrorResult(errors, false));
            }

            var now = DateTime.UtcNow;

            // create a token
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                email = identity.Name
            };

            Response.ContentType = "application/json";
            return Json(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person = _userManager.Validate(username, password);

            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userManager.RegisterUser(model.Email, model.Password);

            if (!string.IsNullOrEmpty(result.Message))
            {
                var errors = new List<string>() { result.Message };
                return GetErrorResult(new ErrorResult(errors, false));
            }

            return Ok();
        }

        [Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            _userManager.Logout(User.Identity.Name);

            return Ok();
        }

        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok(User.Identity.Name);
        }

        #region Helpers

        private ActionResult GetErrorResult(ErrorResult result)
        {
            if (result == null)
            {
                return BadRequest();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("Error", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}