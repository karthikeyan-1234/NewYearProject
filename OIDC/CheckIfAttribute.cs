using Masters.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace OIDC
{
    public class CheckIfAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _resourceName;
        private readonly string[] _allowedScopes;

        public CheckIfAttribute(string? resource, string[] hasScopes)
        {
            _resourceName = resource!;
            _allowedScopes = hasScopes;
        }

        public CheckIfAttribute(string[] hasScopes) : this(null!, hasScopes) { }

        public CheckIfAttribute(string resource)
        {
            _resourceName = resource;
            _allowedScopes = [];
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string authHeader = context.HttpContext.Request.Headers["Authorization"]!;

            var accessToken = authHeader.ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var keycloakService = scope.ServiceProvider.GetRequiredService<IKeycloakAuthorizationService>();

                string resourceToUse = _resourceName; // Resource specified at action level

                // If no resource at action level, try to get from controller level
                if (string.IsNullOrEmpty(resourceToUse))
                {
                    var controllerAttributes = context.ActionDescriptor.FilterDescriptors
                        .Where(filterInfo => filterInfo.Filter is CheckIfAttribute)
                        .Select(filterInfo => filterInfo.Filter as CheckIfAttribute);
                    resourceToUse = controllerAttributes!.FirstOrDefault()?._resourceName!; // Assuming the first CheckIf on the controller defines the default resource
                }

                if (!string.IsNullOrEmpty(resourceToUse) && _allowedScopes != null && _allowedScopes.Length > 0)
                {
                    var user = context.HttpContext.User;
                    var preferredUsernameClaim = user.Claims.FirstOrDefault(c => c.Type == "preferred_username");
                    bool isAuthorized = keycloakService.CheckResourcePermissionAsync(preferredUsernameClaim!.Value, accessToken, _resourceName, _allowedScopes).Result;

                    if (!isAuthorized)
                    {
                        //Return ForbidResult if user is not authorized with authentication scheme, so that the user isn't returned to login screen on authorization failure. To return to login screen, use UnauthorizedResult
                        context.Result = new ForbidResult(); //new UnauthorizedResult();
                    }
                }
                else if (string.IsNullOrEmpty(resourceToUse))
                {
                    // Handle the case where no resource is specified at either level
                    context.Result = new ForbidResult("Resource not specified"); // Or a more appropriate response
                }
            }
        }
    }
}
