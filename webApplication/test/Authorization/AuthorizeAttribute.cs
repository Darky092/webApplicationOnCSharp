using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace webApplication.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<RoleT> _roles;

        public AuthorizeAttribute(params RoleT [] roles)
        {
            _roles = roles ?? new RoleT [] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<AuthorizeAttribute>>();

            logger?.LogDebug("🔍 [Authorize] Starting authorization for action: {Action}",
                context.ActionDescriptor.DisplayName ?? "Unknown");

            var endpointMetadata = context.ActionDescriptor.EndpointMetadata;

            var hasCustomAnon = endpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            var hasBuiltInAnon = endpointMetadata.OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>().Any();

            if (hasCustomAnon || hasBuiltInAnon)
            {
                logger?.LogInformation("✅ [Authorize] Skipped — [AllowAnonymous] present");
                return;
            }

            var account = context.HttpContext.Items.TryGetValue("user", out var u) ? u as user : null;

            logger?.LogDebug("👤 User from Items: {UserId}", account?.userid ?? -1);

            if (account == null)
            {
                logger?.LogWarning("❌ Unauthorized: no user in context");
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            if (_roles.Any() && !_roles.Contains(account.RoleT))
            {
                logger?.LogWarning("🚫 Forbidden: user {Id} (role {Role}) lacks required roles [{Required}]",
                    account.userid, account.RoleT, string.Join(", ", _roles));
                context.Result = new JsonResult(new { message = "Forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            logger?.LogInformation("✅ Access granted for user {Id} (role: {Role})",
                account.userid, account.RoleT);
        }
    }
}