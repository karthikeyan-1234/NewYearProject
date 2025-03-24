using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Masters.Services
{
    public class PermissionsResponse
    {
        public List<PermissionItem>? Permissions { get; set; }
    }

    public record PermissionItem
    {
        [JsonPropertyName("scopes")]
        public List<string>? Scopes { get; set; }

        [JsonPropertyName("rsid")]
        public string? Rsid { get; set; }

        [JsonPropertyName("rsname")]
        public string? Rsname { get; set; }
    }


    public class KeycloakAuthorizationService : IKeycloakAuthorizationService
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KeycloakAuthorizationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> CheckResourcePermissionAsync(string accessToken, string resourceName, string[] allowedScopes)
        {
            var httpClient = _httpClientFactory!.CreateClient();
            var tokenEndpoint = "http://localhost:8080/realms/master/protocol/openid-connect/token";

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = _configuration["Keycloak:GrantType"]!, //"urn:ietf:params:oauth:grant-type:uma-ticket"
                ["audience"] = _configuration["Keycloak:Audience"]!, //"authz"
                ["response_mode"] = "permissions",
                ["permission"] = $"{resourceName}" //#{string.Join(",", allowedScopes)}
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(requestData));

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var permissions = JsonSerializer.Deserialize<List<PermissionItem>>(responseContent);

            var result = permissions?.Any(p =>
                p.Rsname == resourceName &&
                allowedScopes.All(scope => p.Scopes != null && p.Scopes.Contains(scope))) == true;

            return result;
        }
    }
}
