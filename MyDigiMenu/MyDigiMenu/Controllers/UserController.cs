using MyDigiMenu.Attribute;
using MyDigiMenu.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyDigiMenu.Controllers
{
    public class UserController : Controller
    {
        [MyAuthorize]
        [HttpGet]
        public async Task<ActionResult> AllUsers(string filterBy)
        {
            if (Session["Super"].ToString() == "ADMIN")
            {
                AllUserResponse response = await new User().GetAllUsers(filterBy, Session["Token"].ToString(), Session["User"].ToString());

                return View(response);
            }
            return RedirectToAction("Index", "MenuManagement");
        }

        [MyAuthorize]
        public ActionResult ViewUser(string username)
        {

            return View();
        }

        [MyAuthorize]
        public ActionResult CreateUser()
        {
            if (Session["Super"].ToString() == "ADMIN")
            {
                return View();
            }

            return RedirectToAction("Index", "MenuManagement");

        }
    }
}