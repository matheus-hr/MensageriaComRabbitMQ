using Microsoft.AspNetCore.Mvc;
using MessagingEvents.Shared;
using EasyNetQ.Customers.API.Bus;
using EasyNetQ.Customers.API.Models;

namespace EasyNetQ.Customers.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : Controller
    {
        private readonly IBusService _bus;
        private const string ROUTING_KEY = "customer-created";

        public CustomersController(IBusService bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public IActionResult Post(CustomerInputModel model)
        {
            var @event = new CustomerCreated(model.Id, model.FullName, model.Email, model.PhoneNumber, model.BirthDate);

            _bus.Publish(ROUTING_KEY, @event);

            return NoContent();
        }
    }
}
