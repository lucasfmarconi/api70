﻿using FluentResults;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace Api70.Infrastructure.RabbitMq.Connection;

internal class RabbitMqPersistentConnection : IRabbitMqPersistentConnection
{
    private readonly IConnectionFactory connectionFactory;
    private readonly ILogger<RabbitMqPersistentConnection> logger;
    private readonly int retryCount;
    private IConnection connection;
    private bool disposed;

    private readonly object syncRoot = new();

    public RabbitMqPersistentConnection(IConnectionFactory connectionFactory,
        ILogger<RabbitMqPersistentConnection> logger, int retryCount)
    {
        this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.retryCount = retryCount;
    }

    public bool IsConnected => connection is { IsOpen: true } && !disposed;

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

        return connection.CreateModel();
    }

    public void Dispose()
    {
        if (disposed) return;

        disposed = true;

        try
        {
            connection.ConnectionShutdown -= OnConnectionShutdown;
            connection.CallbackException -= OnCallbackException;
            connection.ConnectionBlocked -= OnConnectionBlocked;
            connection.Dispose();
        }
        catch (IOException ex)
        {
            logger.LogCritical(ex.ToString());
        }
    }

    public Result TryConnect()
    {
        logger.LogInformation("RabbitMQ Client is trying to connect");

        lock (syncRoot)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                        $"{time.TotalSeconds:n1}", ex.Message);
                }
            );

            policy.Execute(() =>
            {
                connection = connectionFactory.CreateConnection();
            });

            if (!IsConnected)
            {
                const string errorMessage = "FATAL ERROR: RabbitMQ connections could not be created and opened";
                logger.LogCritical("{message}", errorMessage);
                return Result.Fail(errorMessage);
            }

            connection.ConnectionShutdown += OnConnectionShutdown;
            connection.CallbackException += OnCallbackException;
            connection.ConnectionBlocked += OnConnectionBlocked;
            logger.LogInformation(
                "RabbitMQ Client acquired a persistent connection to '{HostName}'",
                connection.Endpoint.HostName);

            return Result.Ok();
        }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (disposed)
            return;
        logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
        TryConnect();
    }

    private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (disposed)
            return;
        logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
        TryConnect();
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (disposed)
            return;
        logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
        TryConnect();
    }
}
