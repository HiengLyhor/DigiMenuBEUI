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
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [MyAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string name, string email, string message)
        {
            try
            {
                // Handle the form submission, e.g., send an email or save to the database

                // Set a success message
                TempData["SuccessMessage"] = "Your message has been sent successfully.";
            }
            catch
            {
                // Set an error message
                TempData["ErrorMessage"] = "There was an error sending your message. Please try again.";
            }

            // Redirect to the GET action to prevent form resubmission
            return RedirectToAction("Contact");
        }
    
    }
}