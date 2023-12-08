using Microsoft.AspNetCore.Mvc;
using SurfBoardProject.Models;
using System.Diagnostics;

namespace SurfBoardProject.Controllers
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
                throw new Exception("Dette er en fejl");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved behandling af home index");
                return BadRequest("Intern fejl");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}