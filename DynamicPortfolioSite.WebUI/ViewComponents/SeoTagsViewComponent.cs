using Microsoft.AspNetCore.Mvc;

namespace DynamicPortfolioSite.WebUI.ViewComponents
{
    [ViewComponent(Name = "SeoTags")]
    public class SeoTagsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
