using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using rajiunschool.Models;
using Microsoft.Extensions.Logging;

namespace rajiunschool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                // Your logic for the Index action
                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred in the Index action.");

                // Redirect to the custom error page
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                // Your logic for the Privacy action
                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred in the Privacy action.");

                // Redirect to the custom error page
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Log the error (if any)
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError($"Error occurred. Request ID: {requestId}");

            // Return the custom error view
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}