using Microsoft.AspNetCore.Mvc;

namespace DynamicPortfolioSite.WebUI.ViewComponents
{
    [ViewComponent(Name = "AboutSlide")]
    public class AboutSlideViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
