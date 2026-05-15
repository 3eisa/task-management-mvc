using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagementSystem.Controllers;

// Inherit this instead of Controller to protect an action from unauthenticated access.
// Usage: public class UsersController : BaseController { ... }
//        public class TasksController : BaseController { ... }
public abstract class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            context.Result = RedirectToAction("Login", "Account");
            return;
        }
        base.OnActionExecuting(context);
    }
}
