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
                return; // Skip authorization for HomeController.Login
            }

            // Check if the user is authenticated, if not redirect to Login page
            if (filterContext.HttpContext.Session["User"] == null)
            {
                // If not authenticated, redirect to Login page
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }

    }
}