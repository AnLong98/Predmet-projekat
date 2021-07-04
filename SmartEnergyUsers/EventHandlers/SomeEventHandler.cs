using NServiceBus;
using SmartEnergy.Infrastructure;
using SmartEnergyContracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEnergyUsers.EventHandlers
{
    public class DoSomethingHandler : IHandleMessages<SomeEvent>
    {
        UsersDbContext _context;
        public DoSomethingHandler(UsersDbContext context)
        {
            _context = context;
        }

        public Task Handle(SomeEvent message, IMessageHandlerContext context)
        {
            var nesto = _context.Users.ToList();
            return Task.CompletedTask;
        }
    }
}
