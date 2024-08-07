using System.Text.Json;
using Common;
using StackExchange.Redis;

namespace ConsumerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private static readonly string ConnectionString = "localhost:6379";
    private const string Channel = "messages";
    
    private static readonly ConnectionMultiplexer Connection = 
        ConnectionMultiplexer.Connect(ConnectionString);
    
    public Worker(ILogger<Worker> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();

        await subscriber.SubscribeAsync(Channel, (channe, message) =>
            {
                var data = JsonSerializer.Deserialize<Message>(message);
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Received message: {channel} {data}", Channel, data);
                }
            });
    }
}