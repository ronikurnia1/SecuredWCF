namespace ConsoleClient
{
    public static class Config
    {
        // MyService
        public const string TenantId = "[your-tenant-id]";
        public const string AppId = "[your-application-id]";
        public const string AppSecret = "[your-application-secret]";

        public const string IdentityProviderUrl = "https://login.microsoftonline.com";
        public static string AzureADTokenEndpoint = $"{IdentityProviderUrl}/{TenantId}/oauth2/v2.0/token";
        public static string Scope = $"api://{AppId}/[your-application-scope]";

        public const string ServiceEndpoint = "https://localhost:44335/MyService.svc/dummy";

        // Admin User
        public const string AdminName = "[your-admin-user]";
        public const string AdminPassword = "[your-admin-password]";

        // Common User
        public const string UserName = "[your-common-user]";
        public const string UserPassword = "[your-common-user-password]";

        // Service Principal
        public const string ServicePrincipalAppId = "[your-service-principal-appId]";
        public const string ServicePrincipalSecret = "[your-service-principal-secret]";
    }
}
