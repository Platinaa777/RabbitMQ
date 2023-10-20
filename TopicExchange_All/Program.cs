using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange", ExchangeType.Topic);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName,exchange:"topic-exchange", routingKey: "*.*");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"both gave message: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer:consumer);

Console.ReadKey();