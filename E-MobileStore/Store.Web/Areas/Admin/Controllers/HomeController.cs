using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Store.Web.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Route("quan-tri-vien")]
        public IActionResult Index()

        {
            var jwt = HttpContext.Session.GetString("jwtadmin");
            if (jwt == null)
            {
                return RedirectToRoute("login");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
