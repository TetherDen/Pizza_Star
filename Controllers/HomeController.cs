using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lesson_22_Pizza_Star.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



    }
}
