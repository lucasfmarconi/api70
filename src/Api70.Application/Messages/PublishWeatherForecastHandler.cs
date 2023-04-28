using Api70.Core.Messages;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Api70.Application.Messages;
internal class PublishWeatherForecastHandler : IRequestHandler<PublishWeatherForecastCommand, Result>
{
    private readonly IMessagePublisher messagePublisher;

    public PublishWeatherForecastHandler(IMessagePublisher messagePublisher, ILogger<PublishWeatherForecastHandler> logger)
        => this.messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));

    public Task<Result> Handle(PublishWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        var objectJsonString = JsonSerializer.Serialize(request.WeatherForecast);
        var json = JsonDocument.Parse(objectJsonString);
        return Task.FromResult(messagePublisher.PublishMessage(json));
    }
}
