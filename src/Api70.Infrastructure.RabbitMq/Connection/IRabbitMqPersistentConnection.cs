using FluentResults;
using RabbitMQ.Client;
using System;

namespace Api70.Infrastructure.RabbitMq.Connection;
internal interface IRabbitMqPersistentConnection : IDisposable
{
    bool IsConnected { get; }

    Result TryConnect();

    IModel CreateModel();
}