using System;
using FluentResults;
using RabbitMQ.Client;

namespace Api70.Infrastructure.RabbitMq;
internal interface IRabbitMqPersistentConnection : IDisposable
{
    bool IsConnected { get; }

    Result TryConnect();

    IModel CreateModel();
}