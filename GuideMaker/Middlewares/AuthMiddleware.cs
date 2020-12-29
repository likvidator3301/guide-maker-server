using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace GuideMaker.Middlewares
{
    internal class AuthMiddleware: IMiddleware
    {
        private readonly IUserService userService;

        public AuthMiddleware(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path == "/sign-up" || context.Request.Path == "/sign-in")
            {
                var claims = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("IsAuthenticated", "false")
                });
                context.User = new ClaimsPrincipal(claims);
                await next(context);
            }
            else
            {
                var tokens = context.Request.Headers["X-PRIVATE-TOKEN"];
                if (tokens.Count == 0)
                {
                    var result = Result<TokenInformation>.Error("Token missed");
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    return;
                }

                if (tokens.Count > 1)
                {
                    var result = Result<TokenInformation>.Error("Found more than one token");
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    return;
                }

                var token = tokens.First();
                var user = (await userService.GetByTokenAsync(token)).GetValue();

                var claims = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("IsAuthenticated", "true"),
                    new Claim("UserId", user.Id)
                });
                context.User = new ClaimsPrincipal(claims);

                await next(context);
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
