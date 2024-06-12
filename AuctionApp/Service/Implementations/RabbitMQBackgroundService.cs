using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQBackgroundService> _logger;

    public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory() {
            HostName = "localhost"
            //Port = 15672,
            //UserName = "guest",
            //Password = "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        DeclareQueues();
    }

    private void DeclareQueues()
    {
        string[] queues = {
            "start_auction_queue",
            "bid_submitted_queue",
            "bid_updated_queue",
            "highest_bid_queue",
            "end_auction_queue",
            "auction_result_queue"
        };

        foreach (var queue in queues)
        {
            _channel.QueueDeclare(queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ Background Service is starting.");

        stoppingToken.Register(() =>
            _logger.LogInformation("RabbitMQ Background Service is stopping."));

        Consume("start_auction_queue", stoppingToken);
        Consume("bid_submitted_queue", stoppingToken);
        Consume("bid_updated_queue", stoppingToken);
        Consume("highest_bid_queue", stoppingToken);
        Consume("end_auction_queue", stoppingToken);
        Consume("auction_result_queue", stoppingToken);
        

        return Task.CompletedTask;
    }

    private void Consume(string queueName, CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var formattedMessage = JToken.Parse(message).ToString(Formatting.Indented);

            _logger.LogInformation($"Received from {queueName}: {formattedMessage}");
        };

        _channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);

        stoppingToken.Register(() => _channel.Dispose());
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
