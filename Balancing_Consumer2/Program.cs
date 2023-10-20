using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("chat", durable: false, exclusive: false, autoDelete: false);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: true);

var consumer = new EventingBasicConsumer(channel);
Console.WriteLine("consumer 2 is ready");
consumer.Received += (model, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Thread.Sleep(2000);
    Console.WriteLine($"consumer 2 gave {message}");
    
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

channel.BasicConsume("chat", autoAck: false, consumer: consumer);

Console.ReadKey();