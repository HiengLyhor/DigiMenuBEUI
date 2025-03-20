using MyDigiMenu.Models;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Web.Security;
using System.Threading.Tasks;

namespace MyDigiMenu.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToLocal("~/");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["User"] != null) return RedirectToAction("Index", "Home");
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginModel login)
        {
            if (Session["User"] != null) return Json(new { success = true });

            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return Json(new { success = false, message = "Username and password are required." });
            }

            LoginResponse loginResponse = await login.loginUserToAPI(login);

            // Login success & not lock
            if (loginResponse.Code == 200 && loginResponse.IsLock != "Y")
            {
                Session["User"] = loginResponse.Username;
                Session["Token"] = loginResponse.Token;
                Session["CreateDate"] = loginResponse.CreateDate;
                Session["ExpireDate"] = loginResponse.ExpDate;
                Session["Super"] = loginResponse.IsSpecial; // Y & N
                Session["Lock"] = loginResponse.IsLock; // Y & N

                string userData = JsonConvert.SerializeObject(login);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                login.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(5),
                false, //pass here true, if you want to implement remember me functionality
                userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(encTicket, false)));

                // Return success response
                return Json(new { success = true });
            }
            else
            {
                // Return failure response
                return Json(new { success = false, message = "Invalid username or password." });
            }

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
        }

    }
}