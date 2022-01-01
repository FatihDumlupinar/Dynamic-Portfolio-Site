using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

            return View("Default", image);
        }
    }
}
