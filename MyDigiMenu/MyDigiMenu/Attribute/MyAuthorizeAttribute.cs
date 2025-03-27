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

            // Check if the user is authenticated, if not redirect to Login page
            if (filterContext.HttpContext.Session["User"] == null)
            {
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