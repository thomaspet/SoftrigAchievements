
using Microsoft.AspNetCore.Mvc;
using SoftrigAchievements.Services;

namespace SoftrigAchievements.Controllers;

public class Webhooks
{
    public async static Task<IResult> NewCompanyAddedWebhook(
        HttpRequest request,
        [FromServices] IEconomyHttpService economyHttpService,
        [FromBody] NewCompanyEvent ev)
    {
        await economyHttpService.PushEventplanToCompanyAsync(Guid.Parse(ev.CompanyKey), request.Host.ToString());
        return Results.Ok();
    }

    public static IResult HandleCustomerInvoiceWebhook()
    {

        return Results.Ok();
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
