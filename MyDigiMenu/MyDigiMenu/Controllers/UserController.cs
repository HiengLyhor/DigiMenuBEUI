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
        public async Task<ActionResult> AllUsers(string filterBy, string asc)
        {
            if (Session["Super"].ToString() == "Y")
            {
                AllUserResponse response = await new User().GetAllUsers(filterBy, asc, Session["Token"].ToString(), Session["User"].ToString());

                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }

        [MyAuthorize]
        public ActionResult ViewUser(string username)
        {
            // Implement logic to view user details based on the username
            // For example, you can fetch the user details from the database or API
            return View();
        }

    }
}