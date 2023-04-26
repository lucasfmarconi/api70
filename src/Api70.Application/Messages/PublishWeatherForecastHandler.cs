using Api70.Core.Messages;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application.Messages;
internal class PublishWeatherForecastHandler : SafeHandler<PublishWeatherForecastCommand>
{
    private readonly IMessagePublisher messagePublisher;

    public PublishWeatherForecastHandler(IMessagePublisher messagePublisher, ILogger<PublishWeatherForecastHandler> logger) : base(logger) 
        => this.messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));

    protected override Task<Result> HandleAsync(PublishWeatherForecastCommand request, CancellationToken cancellationToken)
    {
        var objectJsonString = JsonSerializer.Serialize(request.WeatherForecast);
        var json = JsonDocument.Parse(objectJsonString);
        return Task.FromResult(messagePublisher.PublishMessage(json));
    }
}
