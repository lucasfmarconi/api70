using FluentResults;

namespace Api70.Core.Messages;
public interface IBrokerMessagePublisher
{
    Result PublishMessageAsync(byte[] messageByteArray, string routingKey);
}
