using MyDigiMenu.Attribute;
using MyDigiMenu.Models;
using System.Web.Mvc;

namespace MyDigiMenu.Controllers
{
    public class HomeController : Controller
    {

        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}