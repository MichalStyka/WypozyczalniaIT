using Microsoft.AspNetCore.Mvc;

namespace Wypozyczalnia.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Autorzy()
        {
            return View(); 
        }


    }

    
}
