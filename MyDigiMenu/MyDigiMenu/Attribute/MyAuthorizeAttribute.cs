using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyDigiMenu.Attribute
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name.ToLower();
            var actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            var actionsToSkip = new List<string> { "login", "forgotpassword", "verifyotp", "resetpassword" };

            // Skip authorization if the controller is "accountcontroller" and the action is in the actionsToSkip list
            if (controllerName == "accountcontroller" && actionsToSkip.Contains(actionName))
            {
                return; // Skip authorization
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