using MessagingEvents.Shared;
using MessagingEvents.Shared.Services;

namespace MassTransit.Marketing.API.Subscribers
{
    public class CustomerCreatedSubscriber : IConsumer<CustomerCreated>
    {
        public IServiceProvider ServiceProdiver { get; }

        public CustomerCreatedSubscriber(IServiceProvider serviceProvider)
        {
            ServiceProdiver = serviceProvider;
        }

        //Para executar esse metoro, precisa abrir pelo console e rodar o metodo dotnet run
        //E enviar a mensagem usando o projeto MassTransit.Customers.API
        public async Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var @event = context.Message;

            using (var scope = ServiceProdiver.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

                await service.SendEmail(@event.Email, "boas-vindas", new Dictionary<string, string> { { "name", @event.FullName } });
            }
        }
    }
}
