using Melody.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Melody.Filters
{
    public class SubscriptionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedSubscriptionTypes;

        public SubscriptionAuthorizeAttribute(params string[] allowedSubscriptionTypes)
        {
            _allowedSubscriptionTypes = allowedSubscriptionTypes;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetInt32("UserId");

            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Signup", null);
                return;
            }

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MelodyContext>();
                var user = dbContext.UserDetails.Find(userId);

                if (user == null || !_allowedSubscriptionTypes.Contains(user.SubscriptionType))
                {
                    context.Result = new ForbidResult(); // You can also redirect to an Access Denied view if needed
                }
            }
        }
    }
}
