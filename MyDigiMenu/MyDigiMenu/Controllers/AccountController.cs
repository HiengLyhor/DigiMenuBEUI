using MyDigiMenu.Models;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net;
using MyDigiMenu.Attribute;

namespace MyDigiMenu.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToLocal("~/");
        }

        [MyAuthorize]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            if (Session["User"] != null) return RedirectToAction("Active", "Recipe");
            return View(new PasswordReset());
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(PasswordReset passwordReset)
        {

            if (passwordReset.Username == null) return RedirectToAction("ForgotPassword");

            var otpSent = await passwordReset.SendOTP(passwordReset);

            if (otpSent.Code == (int)HttpStatusCode.OK)
            {
                Session["AllowVerify"] = "Allow";
                return RedirectToAction("VerifyOTP", passwordReset);

            }
            TempData["ErrorMessage"] = otpSent.Message;
            return View();
        }

        [HttpGet]
        public ActionResult VerifyOTP(PasswordReset reset, string valid)
        {
            if (Session["AllowVerify"] == null) return RedirectToAction("ForgotPassword");

            reset.OTP = null;
            if (Session["UserReset"] != null)
            {
                reset.Username = Session["UserReset"].ToString();
            }
            else
            {
                Session["UserReset"] = reset.Username;
            }
            return View(reset);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyOTP(PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please enter the 6-digit code" });
            }
            try
            {
                if (Session["UserReset"] != null)
                {
                    passwordReset.Username = Session["UserReset"].ToString();
                }
                else
                {
                    Session["UserReset"] = passwordReset.Username;
                }

                var isOtpValid = await passwordReset.VerifyOTP(passwordReset);

                if (isOtpValid.Code == (int)HttpStatusCode.OK)
                {
                    // 2. If valid, mark username as verified (create temp session/token)
                    Session["OTPVerifiedUsername"] = passwordReset.Username;

                    return Json(new
                    {
                        success = true,
                        message = "Verification successful!",
                        redirectUrl = Url.Action("ResetPassword")
                    });
                }

                return Json(new
                {
                    success = false,
                    message = isOtpValid.Message
                });
            }
            catch (Exception ex)
            {
                await GeneralAction.SendMessageAsync(ex.Message);
                return Json(new
                {
                    success = false,
                    message = "An error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            if (Session["OTPVerifiedUsername"] == null) return RedirectToAction("ForgotPassword");
            string username = Session["OTPVerifiedUsername"].ToString();
            return View(new PasswordReset { Username = username });
            
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(PasswordReset passwordReset)
        {
            if (Session["OTPVerifiedUsername"] == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid username to reset password.",
                    redirectUrl = Url.Action("VerifyOTP")
                });
            }

            if (Session["Submitted"] != null)
            {
                return Json(new
                {
                    success = false,
                    message = "You already submitted the form. Refreshing browser...",
                    redirectUrl = Url.Action("VerifyOTP")
                });
            }

            Session.Remove("OTPVerifiedUsername");
            var resetStatus = await passwordReset.ResetPassword(passwordReset);

            Session["Submitted"] = "Yes";

            if (resetStatus.Code == (int)HttpStatusCode.OK)
            {
                return Json(new
                {
                    success = true,
                    message = "Password Reset successful!",
                    redirectUrl = Url.Action("Login")
                });
            }

            return Json(new
            {
                success = false,
                message = resetStatus.Message,
                redirectUrl = Url.Action("ForgotPassword")
            });

        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginModel login = new LoginModel();
            if (Session["User"] != null) return RedirectToAction("Active", "Recipe");
            return View(login);
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginModel login)
        {

            if (Session["User"] != null) return Json(new { success = true });

            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return Json(new { success = false, message = "Username and password are required." });
            }

            LoginResponse loginResponse = await login.LoginUserToAPI(login);

            // Login success & not lock
            if (loginResponse.Code == (int)HttpStatusCode.OK)
            {

                bool isActive = loginResponse.Active.Value;
                if (!isActive)
                {
                    return Json(new { success = false, message = "Your account is locked. Please contact the administrator." });
                }

                string status = "N";
                if (isActive) status = "Y";

                Session["User"] = loginResponse.Username;
                Session["Token"] = loginResponse.Token;
                Session["CreateDate"] = loginResponse.CreateDate;
                Session["ExpireDate"] = loginResponse.ExpDate;
                Session["Super"] = loginResponse.Role; // ADMIN & USER
                Session["Lock"] = status; // Y & N
                Session["ShopName"] = loginResponse.ShopName;
                Session["ProfilePicture"] = loginResponse.ImgUrl;
                Session["TokenExp"] = loginResponse.TokenExp;

                string userData = JsonConvert.SerializeObject(login);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                login.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(9),
                false, //pass here true, if you want to implement remember me functionality
                userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(encTicket, false)));

                // Return success response
                bool isAdmin = loginResponse.Role.Equals("ADMIN");
                return Json(new { success = true, admin = isAdmin });
            }
            else
            {
                // Return failure response
                return Json(new { success = false, message = loginResponse.Message });
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