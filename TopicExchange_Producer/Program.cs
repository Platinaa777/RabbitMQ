using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange", ExchangeType.Topic);

var string1 = "for analytics";
var body1 = Encoding.UTF8.GetBytes(string1);

channel.BasicPublish("topic-exchange", "Russia.analytics", body: body1);
Console.WriteLine("for analytics is sent");

var string2 = "for USA";
var body2 = Encoding.UTF8.GetBytes(string2);

channel.BasicPublish("topic-exchange", "USA.business", body: body2);
Console.WriteLine("for USA is sent");