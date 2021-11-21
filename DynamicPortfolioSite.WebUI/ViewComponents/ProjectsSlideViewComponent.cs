using Microsoft.AspNetCore.Mvc;

namespace DynamicPortfolioSite.WebUI.ViewComponents
{
    [ViewComponent(Name = "ProjectsSlide")]
    public class ProjectsSlideViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
