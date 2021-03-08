using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmpManagmentWebApi.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            bool IsAdminWithEditPolicy = false;
            var claimValue = context.User.Claims.Where(x => x.Type == "Edit Role" && x.Value == "Edit Role").Select(y => y.Value).FirstOrDefault();
            if (context.User.IsInRole("Admin") && claimValue == "Edit Role")
            {
                IsAdminWithEditPolicy = true;
            }
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null)
            {
                if (IsAdminWithEditPolicy == true)
                {
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
            var adminIdBeingEdited = authFilterContext.HttpContext.Request.RouteValues.Where(x => x.Key == "id").Select(y => y.Value).FirstOrDefault();
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (IsAdminWithEditPolicy == true && Convert.ToString(adminIdBeingEdited).ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
