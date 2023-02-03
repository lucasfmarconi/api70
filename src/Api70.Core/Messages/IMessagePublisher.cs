using FluentResults;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Core.Messages;
public interface IMessagePublisher
{
    Task<Result> PublishMessageAsync(JsonDocument message, CancellationToken cancellationToken);
}
