using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServiceContract
{
    public static class ServiceBinding
    {
        public static Binding CreateBinding()
        {
            var binding = new WS2007FederationHttpBinding
                (WSFederationHttpSecurityMode.TransportWithMessageCredential);

            // only for testing on localhost
            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;
            binding.CreateBindingElements().Add(new HttpsTransportBindingElement());

            return binding;
        }

    }
}
