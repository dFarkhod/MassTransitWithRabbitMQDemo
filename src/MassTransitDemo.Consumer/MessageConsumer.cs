using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.Common;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.Consumer
{

    public class MessageConsumer : IConsumer<MessageDto>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MessageDto> context)
        {
            _logger.LogInformation($"Qabul qilib olingan habar: {context.Message.Content} , jo'natuvchi: {context.Message.Sender}");
            return Task.CompletedTask;
        }
    }
}