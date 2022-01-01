using DynamicPortfolioSite.Entities.Enms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DynamicPortfolioSite.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int UserId { get => GetUserId(); }

        protected DateTime DateTimeNow { get => DateTime.Now; }

        private int GetUserId()
        {
            var httpContextItem = HttpContext.Items[ContextItemEnm.User.ToString()];
            var userId = Convert.ToUInt16(httpContextItem);
            return userId;
        }

    }
}
