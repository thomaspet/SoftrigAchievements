
using Microsoft.AspNetCore.Mvc;
using SoftrigAchievements.Models;
using SoftrigAchievements.Services;
using System.Text.Json.Nodes;

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

    public async static Task<IResult> HandleCustomerInvoiceWebhook([FromBody]JsonNode input, [FromServices]IAchievementService service)
    {
        var eventType = (string) input["EventType"];
        var companyKey = (Guid)input["CompanyKey"];
        var invoiceID = (int)input["EntityID"];
        if (eventType == "Create")
        {
            await service.EventTriggeredAchievementAsync(companyKey, invoiceID, AchievementType.InvoiceCreated);
        }
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
