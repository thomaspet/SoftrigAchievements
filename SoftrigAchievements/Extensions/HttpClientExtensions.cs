using IdentityModel.Client;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace SoftrigAchievements.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<TokenResponse?> GetUniEconomyTokenAsync(this HttpClient httpClient, IConfiguration config)
        {
            return RequestTokenAsync(httpClient, config);
        }

        private static string CreateClientToken(IConfiguration config, string audience)
        {
            var clientId = config.GetValue<string>("ui:ClientID");
            var certPass = config.GetValue<string>("ui:CertificatePassword");
            var certPath = config.GetValue<string>("ui:CertificatePath");

            var certificate = new X509Certificate2(certPath, certPass);
            var now = DateTime.UtcNow;

            var securityKey = new X509SecurityKey(certificate);
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.RsaSha256
            );

            var token = new JwtSecurityToken(
                clientId,
                audience,
                new List<Claim>()
                {
                    new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim(JwtClaimTypes.Subject, clientId),
                    new Claim(JwtClaimTypes.IssuedAt, now.ToEpochTime().ToString(), ClaimValueTypes.Integer64)
                },
                now,
                now.AddMinutes(1),
                signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        static async Task<TokenResponse?> RequestTokenAsync(HttpClient client, IConfiguration config)
        {
            var scopes = config.GetValue<string>("ui:Scopes");
            var issuer = config.GetValue<string>("ui:Issuer");
            var clientID = config.GetValue<string>("ui:ClientID");

            var disco = await client.GetDiscoveryDocumentAsync(issuer);
            if (disco.IsError)
                throw new Exception(disco.Error);

            var clientToken = CreateClientToken(config, disco.TokenEndpoint);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                Scope = scopes,
                ClientId = clientID,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
                ClientAssertion =
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = clientToken
                }
            });

            if (response.IsError)
                throw new Exception(response.Error);
            return response;
        }
    }
}
