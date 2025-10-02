using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            //skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            //authorization
            var account = (user)context.HttpContext.Items ["user"];
            if (account == null || (_roles.Any() && !_roles.Contains(account.RoleT)))
            {
                //not logged in role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
