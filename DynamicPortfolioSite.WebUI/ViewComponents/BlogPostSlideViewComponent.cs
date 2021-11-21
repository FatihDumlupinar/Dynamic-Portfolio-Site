using Microsoft.AspNetCore.Mvc;

namespace DynamicPortfolioSite.WebUI.ViewComponents
{
    [ViewComponent(Name = "BlogPostSlide")]
    public class BlogPostSlideViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
