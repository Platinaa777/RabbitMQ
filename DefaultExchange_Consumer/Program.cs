using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
};
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "my-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var customer = new EventingBasicConsumer(channel);

        customer.Received += (sender, e) =>
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            Console.WriteLine($"message received - {message}");
        };

        channel.BasicConsume(queue: "my-queue",
            autoAck: true,
            consumer: customer);

        Console.WriteLine($"subscribed to the queue - my-queue");

        Console.ReadKey();
    } 
}