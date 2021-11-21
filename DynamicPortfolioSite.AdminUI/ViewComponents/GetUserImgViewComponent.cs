using DynamicPortfolioSite.AdminUI.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DynamicPortfolioSite.AdminUI.ViewComponents
{
    [ViewComponent(Name = "GetUserImg")]
    public class GetUserImgViewComponent : ViewComponent
    {
        #region Ctor&Fields

        private readonly IConfiguration _config;

        public GetUserImgViewComponent(IConfiguration config)
        {
            _config = config;
        }

        #endregion

        public IViewComponentResult Invoke()
        {
            string image = _config.GetValue<string>("Defaults:DefaultUserImagePath");//default user image

            string profileImg = HttpContext.User.FindFirst(ClaimTypesEnm.UserImg.ToString())?.Value;
            if (!string.IsNullOrEmpty(profileImg))
            {
                image = profileImg;
            }

            return View("Default", image);
        }
    }
}
