using MassTransit;
using MassTransitDemo.Common;

namespace MassTransitDemo.Publisher
{
    public class MessagePublisher : BackgroundService
    {
        private readonly IBus _bus = null;
        private readonly ILogger<MessagePublisher> _logger;
        public MessagePublisher(IBus bus, ILogger<MessagePublisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Yangi habar jo'natyapman...");
                await _bus.Publish(new MessageDto { Id = Guid.NewGuid(), Sender = "Publisher", Content = $"Vaqt: {DateTimeOffset.Now}" });
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
