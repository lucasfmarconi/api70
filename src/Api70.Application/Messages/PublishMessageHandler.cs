using System;
using System.Text.Json;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Api70.Core.Messages;

namespace Api70.Application.Messages;
internal class PublishMessageHandler : IRequestHandler<PublishMessageCommand, Result>
{
    private readonly IMessagePublisher messagePublisher;

    public PublishMessageHandler(IMessagePublisher messagePublisher)
    {
        this.messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
    }
    public Task<Result> Handle(PublishMessageCommand request, CancellationToken cancellationToken)
    {
        var objectJsonString = JsonSerializer.Serialize(request.JsonElementMessage);
        var json = JsonDocument.Parse(objectJsonString);
        return Task.FromResult(messagePublisher.PublishMessage(json));
    }
}
