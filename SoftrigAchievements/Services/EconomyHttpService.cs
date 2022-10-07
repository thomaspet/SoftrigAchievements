using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using SoftrigAchievements.Extensions;
using SoftrigAchievements.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SoftrigAchievements.Services;

public sealed class EconomyHttpService : IEconomyHttpService
{
	private readonly IHttpClientFactory _clientFactory;
	private readonly IConfiguration _config;
	private readonly IMemoryCache _memoryCache;

	public EconomyHttpService(IHttpClientFactory clientFactory, IConfiguration config, IMemoryCache memoryCache)
	{
		_clientFactory = clientFactory;
		_config = config;
		_memoryCache = memoryCache;
	}

	public async Task<bool> PushEventplanToCompanyAsync(Guid companyKey, string currentHost)
	{
		var httpClient = await GetUniEconomyHttpClientAsync(companyKey);

		var eventplan = new Eventplan
		{
			ModelFilter = "CustomerInvoice",
			Name = "Notify-CatchEverything-Webhook-CustomerInvoice",
			OperationFilter = "CUD",
			PlanType = 0,
			Active = true,
			Subscribers = new()
			{
				new EventSubscriber()
				{
					Name = "Notify-CatchEverything-Webhook-CustomerInvoice",
					Endpoint = $"{currentHost}/webhooks/listen/customerinvoice",
					Active = true,
				}
			}
		};

		var apiurl = _config.GetValue<string>("uri:appframework");


		using var message = new HttpRequestMessage(HttpMethod.Post, new Uri($"{apiurl}/api/biz/eventplans"))
		{
			Content = new StringContent(JsonSerializer.Serialize(eventplan), Encoding.UTF8),
		};
        var response = await httpClient.SendAsync(message);
		var responseText = await response.Content.ReadAsStringAsync();

		return true;
    }

	private async Task<HttpClient> GetUniEconomyHttpClientAsync(Guid companyKey)
	{
        var httpClient = _clientFactory.CreateClient("UniEconomy");
        httpClient.DefaultRequestHeaders.CompanyKey(companyKey);
        await SetUniBearerToken(httpClient);
		return httpClient;
    }

	private async Task SetUniBearerToken(HttpClient httpClient)
	{
        if (!_memoryCache.TryGetValue("UniEconomy-AccessToken", out string token))
		{
            var tokenResponse = await httpClient.GetUniEconomyTokenAsync(_config);

            if (tokenResponse is not null)
            {
                _memoryCache.Set("UniEconomy-AccessToken", tokenResponse.AccessToken, TimeSpan.FromSeconds(tokenResponse.ExpiresIn));
                token = tokenResponse.AccessToken;
			}
		}

        httpClient.SetBearerToken(token);
    }
}

public interface IEconomyHttpService
{
	Task<bool> PushEventplanToCompanyAsync(Guid companyKey, string currentHost);
}


public static class HttpRequestHeadersExtensions
{
	public static void CompanyKey(this HttpRequestHeaders headers, Guid? companyKey)
	{
		headers.Add("CompanyKey", companyKey?.ToString());
	}

    public static void CompanyKey(this HttpRequestHeaders headers, string companyKey)
    {
        headers.Add("CompanyKey", companyKey);
    }
}