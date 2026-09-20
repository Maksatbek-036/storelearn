using Microsoft.AspNetCore.Mvc;

namespace StoreYandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Callback()
        {
            return View();
        }
    }
}
