
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoftrigAchievements.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace SoftrigAchievements.Controllers;

public class Webhooks
{
    public async static Task<IResult> NewCompanyAddedWebhook(
        [FromServices] IConfiguration config,
        [FromServices] IHttpClientFactory clientFactory,
        [FromBody] NewCompanyEvent ev)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("uri:appframework") + "/api/biz/users");
        var httpClient = clientFactory.CreateClient();
        var token = await RequestTokenAsync(httpClient, config);

        httpClient.SetBearerToken(token.AccessToken);
        httpRequest.Headers.Add("CompanyKey", ev.CompanyKey);

        var companyUsers = await httpClient.SendAsync(httpRequest);
        var contentString = await companyUsers.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(contentString)?.ToList();

        if (Guid.TryParse(ev.CompanyKey, out var companyKey) && users is not null)
        {
            foreach (var user in users)
            {
                user.CompanyKey = companyKey;
            }
        }

        return Results.Ok();
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

    static async Task<TokenResponse> RequestTokenAsync(HttpClient client, IConfiguration config )
    {
        var scopes = config.GetValue<string>("ui:Scopes");

        var disco = await client.GetDiscoveryDocumentAsync("https://test-login.softrig.com");
        if (disco.IsError) throw new Exception(disco.Error);

        var clientToken = CreateClientToken(config, disco.TokenEndpoint);

        var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            Scope = scopes,
            ClientId = config.GetValue<string>("ui:ClientID"),
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = clientToken
            }
        });

        if (response.IsError) throw new Exception(response.Error);
        return response;
    }
}


public record NewCompanyEntity(
    int PruchaseEventType,
    int ProductId,
    string ProductName,
    string ProductKey,
    string CompanyName,
    string OrganizationNumber,
    string Email,
    string Platform,
    string ApiBaseUrl,
    int ID);

public record NewCompanyEvent(
    string EntityType,
    string EntityName,
    NewCompanyEntity Entity,
    int EntityID,
    string MessageID,
    string SessionID,
    DateTime TimeStamp,
    string CompanyKey,
    string Reason);
