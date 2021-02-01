using IdentityModel.Client;
using IdentityModel.Constants;
using IdentityModel.Extensions;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Claims;
using System.ServiceModel;
using System.Text.Json;
using System.Xml.Linq;

namespace ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("ADMIN USER ---------------------", ConsoleColor.Blue);
            CallMyService(GetJwtUserAccount(Config.AdminName, Config.AdminPassword));

            WriteLine("COMMON USER --------------------", ConsoleColor.Blue);
            CallMyService(GetJwtUserAccount(Config.UserName, Config.UserPassword));

            WriteLine("SERVICE PRINCIPAL --------------", ConsoleColor.Blue);
            CallMyService(GetJwtServicePrincipal());
            Console.ReadLine();
        }


        private static void CallMyService(string jwtToken)
        {
            var xmlToken = WrapJwt(jwtToken);

            var factory = new ChannelFactory<IMyService>(ServiceBinding.CreateBinding(),
                new EndpointAddress(Config.ServiceEndpoint));

            var myService = factory.CreateChannelWithIssuedToken(xmlToken);

            // Call MyService
            string dataId = "001";
            WriteLine($"Create data: {dataId}", ConsoleColor.White);
            try
            {
                // Call MyService.CreateData
                string result = myService.CreateData(dataId);
                WriteLine($"  {result}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"  Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.WriteLine();

            // Read
            WriteLine($"Read data: {dataId}", ConsoleColor.White);
            try
            {
                // Call MyService.ReadData
                string result = myService.ReadData(dataId);
                WriteLine($"  {result}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"  Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.WriteLine();

            // Update
            WriteLine($"Update data: {dataId}", ConsoleColor.White);
            try
            {
                // Call MyService.UpdateData
                string result = myService.UpdateData(dataId);
                WriteLine($"  {result}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"  Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.WriteLine();

            // Delete
            WriteLine($"Delete data: {dataId}", ConsoleColor.White);
            try
            {
                // Call MyService.DeleteData
                string result = myService.DeleteData(dataId);
                WriteLine($"  {result}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine($"  Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.WriteLine();
        }

        private static GenericXmlSecurityToken WrapJwt(string jwt)
        {
            var subject = new ClaimsIdentity("saml");
            subject.AddClaim(new Claim("jwt", jwt));

            var descriptor = new SecurityTokenDescriptor
            {
                TokenType = TokenTypes.Saml2TokenProfile11,
                TokenIssuerName = "urn:wrappedjwt",
                Subject = subject
            };

            var handler = new Saml2SecurityTokenHandler();
            var token = handler.CreateToken(descriptor);

            var xmlToken = new GenericXmlSecurityToken(
                XElement.Parse(token.ToTokenXmlString()).ToXmlElement(),
                null,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                null,
                null,
                null);

            return xmlToken;
        }

        private static string GetJwtUserAccount(string userName, string password)
        {
            var oauth2Client = new TokenClient(Config.AzureADTokenEndpoint, Config.AppId, Config.AppSecret);

            var tokenResponse = oauth2Client.
                RequestResourceOwnerPasswordAsync(userName, password, Config.Scope).Result;

            return tokenResponse.AccessToken;
        }

        private static string GetJwtServicePrincipal()
        {
            HttpClient httpClient = new HttpClient();
            var request = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", Config.ServicePrincipalAppId },
                { "client_secret", Config.ServicePrincipalSecret },
                { "resource", Config.AppId }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/407cb3f4-71e4-46f0-8cc1-b6675bcbcb09/oauth2/token")
            {
                Content = new FormUrlEncodedContent(request)
            };

            var result = httpClient.SendAsync(requestMessage).Result;

            var response = result.Content.ReadAsStringAsync().Result;
            Token token = JsonSerializer.Deserialize<Token>(response);
            return token.access_token;

        }


        private static void WriteLine(string value, ConsoleColor fgColor)
        {
            Console.ForegroundColor = fgColor;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }


    public class Token
    {
        public string access_token { get; set; }
    }
}
