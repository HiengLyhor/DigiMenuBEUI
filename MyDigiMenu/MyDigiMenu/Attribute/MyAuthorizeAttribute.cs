using System;
using System.Web.Mvc;

namespace MyDigiMenu.Attribute
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name.ToLower();
            var actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            if (controllerName == "accountcontroller" && actionName == "login")
            {
                return; // Skip authorization for AccountController.Login
            }

            if (controllerName == "accountcontroller" && actionName == "forgotpassword")
            {
                return; // Skip authorization for AccountController.ForgotPassword
            }

            if (controllerName == "accountcontroller" && actionName == "verifyotp")
            {
                return; // Skip authorization for AccountController.ForgotPassword
            }

            if (controllerName == "accountcontroller" && actionName == "resetpassword")
            {
                return; // Skip authorization for AccountController.ForgotPassword
            }

            // Check if the user is authenticated, if not redirect to Login page
            if (filterContext.HttpContext.Session["Token"] == null)
            {
                filterContext.HttpContext.Session.Clear();
                // Redirect to the Login action of the AccountController
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
            }

            var tokenExp = filterContext.HttpContext.Session["TokenExp"] as DateTime?;
            if (tokenExp.HasValue && tokenExp.Value < DateTime.Now)
            {
                filterContext.HttpContext.Session.Clear();
                // Redirect to the Login action of the AccountController
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
            }
        }

    }
}