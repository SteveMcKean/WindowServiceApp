using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace CacheInvalidationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly IServiceProvider serviceProvider;

    private static readonly string ConnectionString = "localhost:6379";
   
    private static readonly ConnectionMultiplexer Connection = 
        ConnectionMultiplexer.Connect(ConnectionString);
    
    public const string Channel = "cache-invalidation";
    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = Connection.GetSubscriber();
        await subscriber.SubscribeAsync(Channel, (channel, key) =>
            {
                var cache = serviceProvider.GetRequiredService<IMemoryCache>();
                cache.Remove(key);
                
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Cache item removed: {key}", key);
                }
                
            });
    }
}