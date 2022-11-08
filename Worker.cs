
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using work_process.Models;
using wrk_process.Consumers.Interfaces;
using wrk_process.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace wrk_process;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHabbitMQ _habbitMQ;
    private readonly IOrderRepository _orderRepository;

    private readonly IRedisRepository _redisRepository;

    private readonly IClientAPIRepository _clientApiRepository;


    public Worker(ILogger<Worker> logger,
        IHabbitMQ habbitMQ,
        IOrderRepository orderRepository,
        IRedisRepository redisRepository,
        IClientAPIRepository clientAPIRepository    
    )
    {
        _logger = logger;
        _habbitMQ = habbitMQ;
        _orderRepository = orderRepository;
        _redisRepository = redisRepository;
        _clientApiRepository = clientAPIRepository;
    }

    private async Task SendPostRequest(Guid orderId, ClientAPI clientAPI){
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient httpClient = new HttpClient(clientHandler);

        var json = JsonSerializer.Serialize(new{
            OrderId = orderId
        });

        var data = new StringContent(json, Encoding.UTF8, "application/json");

        httpClient.BaseAddress = new Uri(clientAPI.URLBase);

        //envia para o webhook
        using var httpResponseMessage = await httpClient.PostAsync(clientAPI.URLComplement, data);

        if (!httpResponseMessage.IsSuccessStatusCode)
            throw new Exception("Erro send post information");
    }

     protected override async Task ExecuteAsync(CancellationToken stoppingToken){

        try{
            _logger.LogInformation($"{DateTimeOffset.Now} - Read Habbit");
            var rabbitQueue = Environment.GetEnvironmentVariable("HabbitTopicProcess");
            var factory = await _habbitMQ.RetConnection();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: rabbitQueue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += async (model, ea) =>
            {                
                var body = ea.Body.ToArray();
                var message =  JsonSerializer.Deserialize<string>(body);

                //primeiro receber a msg e consultar no mongo
                var order = await _orderRepository.ReturnOrder(message);

                if (order == null)
                    throw new Exception("Order not found!!!!");


                ClientAPI clientAPI;
                var redis = await _redisRepository.GetStringData(order.ClientId.ToString());
                if (redis == null){
                    
                    clientAPI = await _clientApiRepository.retClientAPIAddress(order.ClientId);

                    if (clientAPI == null)
                        throw new Exception("Nenhuma API Cadastrada para esse cliente");
                        
                    await _redisRepository.SetStringData(clientAPI.Id, JsonSerializer.Serialize(clientAPI));
                }else{
                    clientAPI = JsonSerializer.Deserialize<ClientAPI>(redis);
                }

                await SendPostRequest(order.OrderId, clientAPI);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicQos(0,1,false);
            channel.BasicConsume(queue: rabbitQueue,
                                autoAck: false,
                                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }catch(Exception ex){
            _logger.LogError(ex, "General Error");
        }
     }
}
