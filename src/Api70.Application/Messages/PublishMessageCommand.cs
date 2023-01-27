using FluentResults;
using System.Text.Json;
using MediatR;

namespace Api70.Application.Messages;

public record PublishMessageCommand(object JsonElementMessage) : IRequest<Result>;
