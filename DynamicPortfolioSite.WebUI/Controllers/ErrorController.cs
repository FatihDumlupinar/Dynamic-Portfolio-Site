using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynamicPortfolioSite.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        #region Ctor&Fields

        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
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
