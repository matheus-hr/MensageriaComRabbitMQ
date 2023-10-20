using MessagingEvents.Shared;
using MessagingEvents.Shared.Services;
using Newtonsoft.Json;

namespace EasyNetQ.Marketing.API.Subscribers
{
    public class CustomerCreatedSubcriber : IHostedService
    {
        const string CUSTOMER_CREATED_QUEUE = "customer-created";

        private readonly IAdvancedBus _busAdvanced;

        public IServiceProvider Services { get; }

        public CustomerCreatedSubcriber(IServiceProvider services, IBus bus)
        {
            Services = services;
            _busAdvanced = bus.Advanced;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var queue = _busAdvanced.QueueDeclare(CUSTOMER_CREATED_QUEUE);

            _busAdvanced.Consume<CustomerCreated>(queue, async (msg, info) =>
            {
                var json = JsonConvert.SerializeObject(msg.Body);

                await SendEmail(msg.Body);

                Console.WriteLine($"Message received {json}");
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken) { }

        private async Task SendEmail(CustomerCreated @event)
        {
            using (var scope = Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

                await service.SendEmail(@event.Email, CUSTOMER_CREATED_QUEUE, new Dictionary<string, string> { { "name", @event.FullName } });
            }
        }
    }
}
