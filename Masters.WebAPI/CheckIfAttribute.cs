using Masters.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Masters.WebAPI
{
    public class CheckIfAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _resourceName;
        private readonly string[] _allowedScopes;
        private readonly IServiceProvider? _serviceProvider;

        public CheckIfAttribute(string resource, string[] hasScopes)
        {
            _resourceName = resource;
            _allowedScopes = hasScopes;
        }

        //public CheckIfAttribute(string resource) : this(resource, new string[] { }) { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var accessToken = context.HttpContext.Request.Headers.Authorization.ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var keycloakService = scope.ServiceProvider.GetRequiredService<IKeycloakAuthorizationService>();
                bool isAuthorized = keycloakService.CheckResourcePermissionAsync(accessToken, _resourceName, _allowedScopes).Result;

                if (!isAuthorized)
                {
                    //Return ForbidResult if user is not authorized with authentication scheme
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
