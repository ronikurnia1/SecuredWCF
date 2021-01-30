using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DummyService.Identity
{
    public class IdentityServerWrappedJwtHandler : Saml2SecurityTokenHandler
    {

        private readonly string _issuerName;
        private readonly ICollection<SecurityKey> _signingKeys;
        private readonly ICollection<SecurityToken> _signingTokens;

        public IdentityServerWrappedJwtHandler()
        {
            var configurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(Config.OpenIdConnectUrl);

            var config = configurationManager.GetConfigurationAsync().Result;
            _issuerName = config.Issuer;
            _signingKeys = config.SigningKeys.ToList();
            _signingTokens = config.SigningTokens.ToList();            
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            var saml = token as Saml2SecurityToken;
            var samlAttributeStatement = saml.Assertion.Statements.OfType<Saml2AttributeStatement>().FirstOrDefault();

            var jwt = samlAttributeStatement.Attributes
                .Where(sa => sa.Name.Equals("jwt", StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault().Values.Single();

            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _issuerName,
                IssuerSigningKeys = _signingKeys,
                ValidateLifetime = false,
                IssuerSigningTokens = _signingTokens,
                IssuerSigningKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.AppSecret))
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(jwt, parameters, out SecurityToken validatedToken);

            var claimsIdentities = new ReadOnlyCollection<ClaimsIdentity>(new List<ClaimsIdentity> { principal.Identities.First() });

            return claimsIdentities;
        }
    }
}