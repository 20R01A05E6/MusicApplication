using Melody.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Melody.Authorization
{
    public class SubscriptionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _subscriptionType;

        public SubscriptionAuthorizeAttribute(string subscriptionType)
        {
            _subscriptionType = subscriptionType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionUserId = context.HttpContext.Session.GetInt32("UserId");

            // Check if user is logged in
            if (sessionUserId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Signup", null);
                return;
            }

            // Fetch user from database
            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MelodyContext>();
                var user = dbContext.UserDetails.Find(sessionUserId);

                // Check if user exists and subscription type matches
                if (user == null || user.SubscriptionType != _subscriptionType)
                {
                    context.Result = new ForbidResult(); // Or redirect to an access denied page
                }
            }
        }
    }
}
