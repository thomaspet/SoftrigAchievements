using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SoftrigAchievements;

public static class AuthenticationExtensions
{
    public static void AddUniAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var issuer = configuration["ui:Issuer"];
        var validIssuers = configuration.GetSection("ui:ValidIssuers").Get<string[]>();
        var audienceName = configuration["ui:AudienceName"];



        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
             {
                 o.ClaimsIssuer = issuer;
                 o.Audience = audienceName;
                 o.Authority = issuer;
                 o.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuers = validIssuers,
                     ValidateIssuer = true,
                     ValidAudience = audienceName,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,
                     RequireSignedTokens = true,
                     RequireExpirationTime = true,
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.FromMinutes(5),
                     SaveSigninToken = false,
                 };
             });
    }
}
