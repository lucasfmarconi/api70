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
    public async Task<Result> PublishMessageAsync(JsonElement message, CancellationToken cancellationToken)
    {
        logger.BeginScope($"{nameof(MessagePublisher)}:{Guid.NewGuid()}");
        {
            logger.LogDebug("Publishing message {@message}", message);
            await Task.Run(() =>
            {
                var randomTimeMs = Random.Shared.Next(35, 150);
                Thread.Sleep(randomTimeMs);
            }, cancellationToken);
            return Result.Ok();
        }
    }
}
