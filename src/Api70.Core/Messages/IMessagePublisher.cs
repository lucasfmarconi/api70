using FluentResults;
using System.Text.Json;

namespace Api70.Core.Messages;
public interface IMessagePublisher
{
    Result PublishMessage(JsonDocument message);
}
