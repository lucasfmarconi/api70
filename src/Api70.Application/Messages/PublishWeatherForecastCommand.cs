using Api70.Core.Domain;
using FluentResults;
using MediatR;

namespace Api70.Application.Messages;

public record PublishWeatherForecastCommand(WeatherForecast WeatherForecast) : IRequest<Result>;
