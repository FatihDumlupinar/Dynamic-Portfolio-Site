using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicPortfolioSite.WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Ctor&Fields

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

    }
}
