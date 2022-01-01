using DynamicPortfolioSite.Core.Constants;
using DynamicPortfolioSite.Core.Utilities.Helpers;
using DynamicPortfolioSite.Entities.Enms;
using DynamicPortfolioSite.Entities.Models.Account;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #endregion

        #region Login
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginApiModel model)
        {
            var checkUser = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && (i.UserName == model.EmailOrUserName || i.Email == model.EmailOrUserName) && i.PasswordHash == HashingHelper.CreateMD5Hash(model.Password));
            if (checkUser == default)
            {
                return NotFound(Messages.UserNotFound);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypesEnm.UserId.ToString(), checkUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenString = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(tokenString);

            return Ok(token);
        }

        #endregion

        #region Auth

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("auth")]
        public IActionResult Auth()
        {
            return Ok();
        }

        #endregion

    }
}
