using System.Text;
using System.Text.Json;
using RabbitMQClient.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string EXCHANGE = "curso-rabbitmq";
const string QUEUE_FILA = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Matheus Henrique Rodrigues", "123.456.789-00", new DateTime(1998,04,21));

var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = connectionFactory.CreateConnection("curso-rabbitmq");

var channel = connection.CreateModel();

var json = JsonSerializer.Serialize(person);
var byteArray = Encoding.UTF8.GetBytes(json);

channel.BasicPublish(EXCHANGE, ROUTING_KEY, null, byteArray);

Console.WriteLine($"Message published: {json}");

var consumerChannel = connection.CreateModel();

var consumer = new EventingBasicConsumer(consumerChannel);

consumer.Received += (sender, eventArgs) =>
{
    var contentArray = eventArgs.Body.ToArray();
    var contentString = Encoding.UTF8.GetString(contentArray);

    var message = JsonSerializer.Deserialize<Person>(contentString);

    Console.WriteLine($"Message received: {contentString}");

    consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
};

consumerChannel.BasicConsume(QUEUE_FILA, false, consumer);

Console.ReadLine();