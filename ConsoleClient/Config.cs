namespace ConsoleClient
{
    public static class Config
    {
        // MyService
        public const string TenantId = "407cb3f4-71e4-46f0-8cc1-b6675bcbcb09";
        public const string AppId = "c2b34b51-8fd7-4cbc-886f-bb0973135d24";
        public const string AppSecret = "TBw5w1_uTIzc20t0nl6-7use_xnDv~nKi3";

        public const string IdentityProviderUrl = "https://login.microsoftonline.com";
        public static string AzureADTokenEndpoint = $"{IdentityProviderUrl}/{TenantId}/oauth2/v2.0/token";
        public static string Scope = $"api://{AppId}/DummyServiceScope";

        public const string ServiceEndpoint = "https://localhost:44335/MyService.svc/dummy";

        // Admin User
        public const string AdminName = "admin@smartdomain.onmicrosoft.com";
        public const string AdminPassword = "StartNewDay2@";

        // Common User
        public const string UserName = "user@smartdomain.onmicrosoft.com";
        public const string UserPassword = "StartNewDay2@";

        // Service Principal
        public const string ServicePrincipalAppId = "fb6062d2-55bd-4cb9-b0b0-6bec695abd4c";
        public const string ServicePrincipalSecret = "GDXG_n94b2b3aU_i4ox1ce55kC7Qa~-WW5";
    }
}
