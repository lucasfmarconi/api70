﻿using FluentResults;
using System.Text.Json;

namespace Api70.Core.Messages;
public interface IMessagePublisher
{
    Task<Result> PublishMessageAsync(JsonDocument message, CancellationToken cancellationToken);
}
