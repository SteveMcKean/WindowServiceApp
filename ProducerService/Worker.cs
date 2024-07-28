using System.Text.Json;
using Common;
using StackExchange.Redis;

namespace ProducerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private static readonly string ConnectionString = "localhost:6379";

    private static readonly ConnectionMultiplexer Connection =
        ConnectionMultiplexer.Connect(ConnectionString);

    private const string Channel = "messages";
    public Worker(ILogger<Worker> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        while (!stoppingToken.IsCancellationRequested)
        {
            var message = new Message(Guid.NewGuid(), DateTime.UtcNow, "Message content");
            var json = JsonSerializer.Serialize(message);

            await subscriber.PublishAsync(Channel, json);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Sending message: {channel} {message}", Channel, json);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}