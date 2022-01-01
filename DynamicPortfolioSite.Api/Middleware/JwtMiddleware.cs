using DynamicPortfolioSite.Entities.Enms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly IConfiguration _configuration;

        public JwtMiddleware(IConfiguration configuration, RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].SingleOrDefault()?.Split(" ").Last();//Jwt Bearer

            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.Items[ContextItemEnm.User.ToString()] = Convert.ToUInt16(jwtToken.Claims.Single(x => x.Type == ClaimTypesEnm.UserId.ToString()).Value);
            }

            await _next(context);
        }


    }
}
