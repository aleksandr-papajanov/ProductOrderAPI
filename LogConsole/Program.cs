using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var queueName = "OrderStatusUpdate";
var hostName = "rabbitmq";

var factory = new ConnectionFactory() { HostName = hostName };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) => 
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    Console.WriteLine($"Order status updated: {message}");

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync(
    queue: queueName,
    autoAck: true,
    consumer: consumer
);

Console.WriteLine("Listening...");
Console.ReadLine();