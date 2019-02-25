﻿using System;
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
    /// <summary>
    /// Authentication controller used by UI
    /// </summary>
    public class AccountController : Controller
    {
        //TODO: Move to the config
        private readonly UserManager _userManager = new UserManager(@"https://localhost:44320/api/");

        /// <summary>
        /// API JWT based authentication  
        /// </summary>
        /// <returns>Encoded JWT token and user email</returns>
        [HttpPost("token")]
        public ActionResult Token()
        {
            var email = Request.Form["email"];
            var password = Request.Form["password"];

            // User validation
            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                var errors = new List<string>() { "Invalid username or password." };
                return GetErrorResult(new ErrorResult(errors, false));
            }

            var now = DateTime.UtcNow;

            // Create a token
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

        /// <summary>
        /// Validates user based on email and password
        /// </summary>
        /// <param name="email">User emaii. Is used as a username</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        private ClaimsIdentity GetIdentity(string email, string password)
        {
            var userProfile = _userManager.Validate(email, password);

            if (userProfile != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userProfile.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

        /// <summary>
        /// Registers a user profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Logs out the user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            _userManager.Logout(User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Logged user username/email
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok(User.Identity.Name);
        }

        #region Helpers
        
        /// <summary>
        /// UI errors
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
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