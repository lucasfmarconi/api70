using Api70.Core.Messages;
using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Api70.Infrastructure.Messages;
internal class MessagePublisher : IMessagePublisher
{
    private readonly ILogger<MessagePublisher> logger;

    public MessagePublisher(ILogger<MessagePublisher> logger)
    {
        this.logger = logger;
    }
    public async Task<Result> PublishMessageAsync(JsonDocument message, CancellationToken cancellationToken)
    {
        logger.BeginScope($"{nameof(MessagePublisher)}:{Guid.NewGuid()}");
        {
            logger.LogDebug("Publishing message {message}", message.RootElement.ToString());
            await Task.Run(() =>
            {
                var randomTimeMs = Random.Shared.Next(1000, 30000);
                Thread.Sleep(randomTimeMs);
                logger.LogInformation("Message published after {randomTimeMs}", randomTimeMs);
            }, cancellationToken);
            return Result.Ok();
        }
    }
}
