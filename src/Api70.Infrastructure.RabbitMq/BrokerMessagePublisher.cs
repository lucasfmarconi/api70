﻿using Api70.Infrastructure.Messages;
using FluentResults;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Api70.Infrastructure.RabbitMq;

internal class BrokerMessagePublisher : IBrokerMessagePublisher
{
    private readonly ILogger<BrokerMessagePublisher> logger;
    private readonly IRabbitMqPersistentConnection persistentConnection;
    private const string ExchangeName = "Api70";

    public BrokerMessagePublisher(ILogger<BrokerMessagePublisher> logger,
        IRabbitMqPersistentConnection persistentConnection)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.persistentConnection =
            persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
    }


    public Result PublishMessageAsync(byte[] messageByteArray, string routingKey = "api70.all")
    {
        logger.LogDebug("Message being published to broker.");

        if (messageByteArray == null) 
            return Result.Fail("Message can not be null");

        logger.LogDebug("Message will be published to the broker {@messageByteArray}", messageByteArray);
        
        using var channel = persistentConnection.CreateModel();

        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic);
        
        channel.BasicPublish(exchange: ExchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: messageByteArray);

        logger.LogTrace("Message sent");

        return Result.Ok();
    }
}
