/*using Melody.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Melody.Authorization
{
    public class SubscriptionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _subscriptionTypes; // Array of allowed subscription types

        public SubscriptionAuthorizeAttribute(params string[] subscriptionTypes)
        {
            _subscriptionTypes = subscriptionTypes;
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

            // Fetch user from the database
            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MelodyContext>();
                var user = dbContext.UserDetails.Find(sessionUserId);

                // Check if the user exists and has an allowed subscription type
                if (user == null || !_subscriptionTypes.Contains(user.SubscriptionType))
                {
                    context.Result = new RedirectToActionResult("AccessDenied","Signup",null); // Or redirect to an access denied page
                }
            }
        }
    }
}
*/