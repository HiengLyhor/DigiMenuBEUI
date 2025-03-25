using MyDigiMenu.Attribute;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyDigiMenu.Controllers
{
    public class UserController : Controller
    {
        [MyAuthorize]
        public ActionResult AllUsers()
        {
            if (Session["Super"].ToString() == "Y")
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [MyAuthorize]
        public async Task<JsonResult> AllUsers(string sortBy, bool asc)
        {
            return null;
        }
    }
}