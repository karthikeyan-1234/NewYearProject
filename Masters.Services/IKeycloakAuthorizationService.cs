using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masters.Services
{
    public interface IKeycloakAuthorizationService
    {
        Task<bool> CheckResourcePermissionAsync(string accessToken, string resourceName, string[] allowedScopes);
    }
}
