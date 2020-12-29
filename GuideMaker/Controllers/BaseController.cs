using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuideMaker.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        protected readonly IUserService UserService;

        protected BaseController(IUserService userService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        protected async Task<User> GetUserAsync()
        {
            var isAuthenticated = bool.Parse(HttpContext.User.Claims.First(x => x.Type == "IsAuthenticated").Value);
            if (!isAuthenticated)
                return null;

            var userId = HttpContext.User.Claims.First(x => x.Type == "UserId").Value;

            return (await UserService.GetAsync(userId)).GetValue();
        }
    }
}
