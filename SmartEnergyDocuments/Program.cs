using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SmartEnergyContracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEnergyDocuments
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                    Host.CreateDefaultBuilder(args)
                        /*.UseNServiceBus(context =>
                        {

                            var endpointConfiguration = new EndpointConfiguration("DocumentsEndpoint");
                            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                            transport.ConnectionString("host=rabbitmq");
                            transport.UseConventionalRoutingTopology();
                            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                            endpointConfiguration.EnableInstallers();
                            endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

                            return endpointConfiguration;
                        })*/
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
    }
}
