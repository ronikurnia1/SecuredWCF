using DummyService.Identity;
using ServiceContract;
using System;
using System.IdentityModel.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace DummyService.Extensions
{
    public static class ServiceConfigurationExtension
    {
        public static ServiceConfiguration ApplyCustomAuthentication
            (this ServiceConfiguration config, Type serviceType, string address)
        {
            config.Credentials.IdentityConfiguration = CreateIdentityConfiguration();
            config.Credentials.UseIdentityConfiguration = true;
            config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
            config.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            var behavior = new ServiceAuthorizationBehavior
            {
                PrincipalPermissionMode = PrincipalPermissionMode.Always
            };
            config.Description.Behaviors.Add(behavior);

            config.AddServiceEndpoint(serviceType, ServiceBinding.CreateBinding(), address);
            return config;
        }


        private static IdentityConfiguration CreateIdentityConfiguration()
        {
            var identityConfiguration = new IdentityConfiguration();
            identityConfiguration.SecurityTokenHandlers.Clear();

            identityConfiguration.SecurityTokenHandlers.Add(new IdentityServerWrappedJwtHandler());

            identityConfiguration.ClaimsAuthorizationManager =
                new CustomClaimsAuthorizationManager();

            return identityConfiguration;
        }

    }
}