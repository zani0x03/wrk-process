using RabbitMQ.Client;
using wrk_process.Consumers.Interfaces;

namespace wrk_process.Consumers;


public class HabbitMQ:IHabbitMQ{

    private ConnectionFactory _connectionFactory = null;


    public HabbitMQ(){
        if (_connectionFactory == null){
            _connectionFactory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("HabbitIp") };
        }
    }

    public async Task<ConnectionFactory> RetConnection()
    {
        return _connectionFactory;
    }
}