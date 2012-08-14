using System.Web.Mvc;

namespace HTA.Websites.MVC.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to the Global Adventure Map.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
