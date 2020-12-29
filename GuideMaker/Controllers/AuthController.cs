using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Services;
using Microsoft.AspNetCore.Mvc;
using Guide = GuideMaker.Repository.Models.Guide;

namespace GuideMaker.Controllers
{
    [ApiController]
    public sealed class AuthController: BaseController
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService): base(userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<Result<TokenInformation>>> SingUpAsync([FromBody] UserAuthData userAuthData)
        {
            if (await userService.ExistsAsync(userAuthData.Login))
                return Ok(await userService.SignInAsync(userAuthData));

            return Ok(await userService.SignUpAsync(userAuthData));
        }

        [HttpGet("user")]
        public async Task<User> GetCurrentUserAsync()
        {
            return await GetUserAsync();
        }
    }
}
