using RabbitMQ.Client;

namespace wrk_process.Consumers.Interfaces;


public interface IHabbitMQ
{
    Task<ConnectionFactory> RetConnection();
}