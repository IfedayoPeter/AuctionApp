using System.Text;
using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Domain.DTOs.Bid;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Entities.Auction;

namespace AuctionApp.Service.Implementations;

public class RabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(ILogger<RabbitMQService> logger)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;
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

    public void Publish(string queueName, object message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            _logger.LogInformation($"Published to {queueName}: {JsonConvert.SerializeObject(message, Formatting.Indented)}");
            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error publishing message: {ex.Message}");
        }
    }

    public void PublishAuctionStarted(AuctionDTO auction)
    {
        Publish("start_auction_queue", new { message = "Auction has started", Auction = auction });
    }

    public void PublishBidSubmitted(BidDTO bid)
    {
        Publish("bid_submitted_queue", new { message = "Bid submitted", Bid = bid  });
    }

    public void PublishBidUpdated(BidDTO bid)
    {
        Publish("bid_updated_queue", new { message = "Bid updated", Bid = bid });
    }

    public void PublishCurrentHighestBid(BidDTO bid)
    {
        Publish("highest_bid_queue", new { message = "Current highest bid", Bid = bid });
    }

    public void PublishEndAuction(AuctionDTO auction)
    {
        Publish("end_auction_queue", new { message = "Auction has ended", Auction = auction });
    }

    public void PublishAuctionResult(AuctionResultDTO auctionResult)
    {
        Publish("auction_result_queue", new { message = "Auction Result", Auction = auctionResult });
    }
}