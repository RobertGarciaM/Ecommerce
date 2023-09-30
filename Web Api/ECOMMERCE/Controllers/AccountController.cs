using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ECOMMERCE.Providers;
using ECOMMERCE.Results;
using Models;
using Models.ViewModel;
using Models.User;
using Infrastructure;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Models.Login;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Data.Entity;
using Models.Products;
using System.Net;

namespace ECOMMERCE.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public AccountController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Authenticate a user by providing their credentials.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to authenticate a user by providing their username and password.
        /// </remarks>
        /// <param name="loginModel">The user's login credentials.</param>
        /// <returns>Returns an HTTP response containing an authentication token if the login is successful.</returns>
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindAsync(loginModel.UserName, loginModel.Password);

            if (user != null)
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                });

                foreach (var role in roles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                string secret = ConfigurationManager.AppSettings["JwtTokenSecret"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }

            return BadRequest("Incorrect username or password.");
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to register a new user.
        /// </remarks>
        /// <param name="model">The registration model.</param>
        /// <returns>Returns an HTTP response indicating the registration status.</returns>
        [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(BindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var roleName = model.Role.ToString();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                var createRoleResult = await roleManager.CreateAsync(role);

                if (!createRoleResult.Succeeded)
                {
                    return GetErrorResult(createRoleResult);
                }
            }

            var roleResult = await UserManager.AddToRoleAsync(user.Id, roleName);

            if (!roleResult.Succeeded)
            {
                return GetErrorResult(roleResult);
            }

            return Ok();
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to register a new user.
        /// </remarks>
        /// <param name="model">The registration model.</param>
        /// <returns>Returns an HTTP response indicating the registration status.</returns>
        [AllowAnonymous]
        [Route("AnonimousRegister")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> AnonimousRegister(BindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var roleName = model.Role.ToString();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                var createRoleResult = await roleManager.CreateAsync(role);

                if (!createRoleResult.Succeeded)
                {
                    return GetErrorResult(createRoleResult);
                }
            }

            var roleResult = await UserManager.AddToRoleAsync(user.Id, roleName);

            if (!roleResult.Succeeded)
            {
                return GetErrorResult(roleResult);
            }

            return Ok();
        }


        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrieve a list of all registered users.
        /// </remarks>
        /// <returns>Returns an HTTP response containing a list of users.</returns>
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();
            return Request.CreateResponse(HttpStatusCode.Created, users);
        }

        /// <summary>
        /// Update user information.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to update user information.
        /// </remarks>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="model">The updated user information.</param>
        /// <returns>Returns an HTTP response indicating the update status.</returns>
        [HttpPut]
        [Route("UpdateUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> UpdateUser(string userId, BindingModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to delete a user.
        /// </remarks>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Returns an HTTP response indicating the delete status.</returns>
        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteUser(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await UserManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}
