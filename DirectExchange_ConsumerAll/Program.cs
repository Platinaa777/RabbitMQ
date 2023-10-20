using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() {HostName = "localhost"};
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

        var queueName = channel.QueueDeclare().QueueName;
        
        channel.QueueBind(queue: queueName,
            exchange: "direct_logs",
            routingKey: "warning");
        
        channel.QueueBind(queue: queueName,
            exchange: "direct_logs",
            routingKey: "info");

        var customer = new EventingBasicConsumer(channel);

        customer.Received += (sender, e) =>
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            Console.WriteLine($"message received - {message}");
        };

        channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: customer);

        Console.WriteLine($"subscribed to the queue - {queueName}");
        
        Console.ReadKey();
    } 
}