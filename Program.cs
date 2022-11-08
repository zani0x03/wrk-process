using StackExchange.Redis;
using wrk_process;
using wrk_process.Consumers;
using wrk_process.Consumers.Interfaces;
using wrk_process.Repositories;
using wrk_process.Repositories.Interfaces;

var multiplexer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("Redis"));

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<IHabbitMQ, HabbitMQ>();
        services.AddSingleton<IMongoDBConn, MongoDBConn>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IRedisRepository, RedisRepository>();
        services.AddSingleton<IPostgresDBConn, PostgresDBConn>();
        services.AddSingleton<IClientAPIRepository, ClientAPIAddress>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
