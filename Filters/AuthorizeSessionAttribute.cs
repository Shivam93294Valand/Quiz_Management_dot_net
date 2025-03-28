using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace QuizeManagement_Project.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32("UserID") == null || context.HttpContext.Session.GetInt32("UserID") == 0)
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    controller.TempData["AuthError"] = "Please sign in to your account to access this page.";
                    context.Result = new RedirectToActionResult("LoginAccountForm", "Forms", null); 
                }
            }
            base.OnActionExecuting(context);
        }
    }
}