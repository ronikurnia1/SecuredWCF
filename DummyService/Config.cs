using System.Collections.Specialized;
using System.Configuration;

namespace DummyService
{
    public static class Config
    {
        private static readonly NameValueCollection config = ConfigurationManager.AppSettings;

        public static string TenantId = config["TenantId"];
        public static string AppId = config["AppId"];
        public static string AppSecret = config["AppSecret"];

        public static string OpenIdConnectUrl =
            $"{config["IdentityProviderUrl"]}/{TenantId}/.well-known/openid-configuration";

        public static string Scope = $"api://{AppId}/{config["AppScope"]}";
        public static string ServiceEndpoint = config["ServiceEndpoint"];
    }
}