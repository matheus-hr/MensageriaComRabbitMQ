namespace EasyNetQ.Customers.API.Bus
{
    public class EasyNetQService : IBusService
    {
        private readonly IAdvancedBus _busAdvanced;

        private readonly string EXCHANGE = "curso-rabbitmq";

        public EasyNetQService(IBus bus)
        {
            _busAdvanced = bus.Advanced;
        }

        public void Publish<T>(string routingKey, T message)
        {
            var exchange = _busAdvanced.ExchangeDeclare(EXCHANGE, "topic");
            _busAdvanced.Publish(exchange, routingKey, true, new Message<T>(message));
        }
    }
}
