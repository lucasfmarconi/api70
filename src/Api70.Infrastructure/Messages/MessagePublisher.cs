using Api70.Core.Messages;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;

namespace Api70.Infrastructure.Messages;
internal class MessagePublisher : IMessagePublisher
{
    private readonly ILogger<MessagePublisher> logger;
    private readonly IBrokerMessagePublisher brokerMessagePublisher;

    public MessagePublisher(ILogger<MessagePublisher> logger, IBrokerMessagePublisher brokerMessagePublisher)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.brokerMessagePublisher =
            brokerMessagePublisher ?? throw new ArgumentNullException(nameof(brokerMessagePublisher));
    }

    public Result PublishMessage(JsonDocument message)
    {
        if (message == null)
            return Result.Fail($"{nameof(message)} cannot be null.");

        logger.LogTrace("Message will be sent to broker");

        var byteArray = Encoding.UTF8.GetBytes(message.RootElement.GetRawText());
        return brokerMessagePublisher.PublishMessageAsync(byteArray);
    }
}
