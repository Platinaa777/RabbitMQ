using System.Text;
using RabbitMQ.Client;

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

        string message = "data1231";

        var data = Encoding.UTF8.GetBytes(message);
        
        channel.BasicPublish(exchange: "",
            routingKey: "my-queue",
            basicProperties: null,
            body: data);


        Console.WriteLine("work is done");
    }
}