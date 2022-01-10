using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Models;
using System.Diagnostics;

namespace Studentenbeheer.Controllers
{
    public class HomeController : ApplicationController
    {
        //private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            ApplicationUser user = _user;
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