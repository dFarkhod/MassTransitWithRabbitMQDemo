using MassTransit;
using MassTransitDemo.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MassTransitDemo.Consumer
{
    public class Program
    {
        private static IConfiguration _config = null;
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            _config = builder.Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var rabbitMqSection = _config.GetSection("RabbitMqSettings");
                    var _rabbitMqSettings = rabbitMqSection.Get<RabbitMqSettings>();
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(_rabbitMqSettings.Host, _rabbitMqSettings.VirtualHost, host =>
                            {
                                host.Username(_rabbitMqSettings.Username);
                                host.Password(_rabbitMqSettings.Password);
                            });
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
