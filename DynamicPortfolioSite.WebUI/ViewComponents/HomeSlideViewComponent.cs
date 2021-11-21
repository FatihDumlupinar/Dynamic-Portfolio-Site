using Microsoft.AspNetCore.Mvc;

namespace DynamicPortfolioSite.WebUI.ViewComponents
{
    [ViewComponent(Name = "HomeSlide")]
    public class HomeSlideViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
