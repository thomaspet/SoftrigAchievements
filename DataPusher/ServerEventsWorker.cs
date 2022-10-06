using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;

namespace DataPusher;

public class ServerEventsWorker : IHostedService
{
    private readonly IServerSentEventsService _client;

    public ServerEventsWorker(IServerSentEventsService client)
    {
        _client = client;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var clients = _client.GetClients();
                if (!clients.Any())
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                    continue;
                }

                await _client.SendEventAsync(new ServerSentEvent
                {
                    Id = "number",
                    Type = "number",
                    Data = new List<string> { RandomNumberGenerator.GetInt32(1, 100).ToString() }
                }, cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
        catch (TaskCanceledException) { }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
