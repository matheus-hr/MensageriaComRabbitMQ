using EasyNetQ;
using EasyNetQ.Console;
using Newtonsoft.Json;

const string EXCHANGE = "curso-rabbitmq";
const string QUEUE_FILA = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Matheus Henrique Rodrigues", "123.456.789-00", new DateTime(1998, 04, 21));

var bus = RabbitHutch.CreateBus("host=localhost");

var advanced = bus.Advanced;

var exchange = advanced.ExchangeDeclare(EXCHANGE, "topic");
var queue = advanced.QueueDeclare(QUEUE_FILA);

advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person)); // Cria/Publica a mensagem

advanced.Consume<Person>(queue, (msg, info) => { // Consome a mensagem
    var json = JsonConvert.SerializeObject(msg.Body);
    Console.WriteLine(json);
});

Console.ReadLine();

//---------------------------------------------//

//await bus.PubSub.PublishAsync(person); //Publicou para uma exchange - cria uma mensagem

//await bus.PubSub.SubscribeAsync<Person>("marketing", msg => //Cria a fila - consome a mensagem
//{
//    var json = JsonConvert.SerializeObject(msg);
//    Console.WriteLine(json);
//}); //Consumindo mensagem
