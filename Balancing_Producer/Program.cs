using System.Security.AccessControl;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("chat", durable: false, exclusive: false, autoDelete: false);

int messageId = 1;

while (true)
{
    string body = $"sending Message id = {messageId}";
    var data = Encoding.UTF8.GetBytes(body);
    
    Thread.Sleep(5000);
    channel.BasicPublish(exchange: "", routingKey: "chat", body: data, basicProperties:null);
    Console.WriteLine($"message with id {messageId++} is sent");
}