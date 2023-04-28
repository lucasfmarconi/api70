using System;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace Api70.Application.PipelineBehavior.Adapter.PipelineBehaviors.ErrorMonitoringHandlers;

public class ConsoleErrorMonitoringHandler : IErrorMonitoringHandler
{
    private static readonly object SyncRoot = new();

    public Task ReportAsync(Result result)
    {
        lock (SyncRoot)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] {0}", result.Errors.First().Message);
            Console.WriteLine(string.Join(",", result.Errors.First().Reasons.Select(e => e.Message)));
            Console.WriteLine(result.Errors.First().Reasons.OfType<ExceptionalError>().FirstOrDefault()?.Exception);
            Console.ResetColor();
        }

        return Task.CompletedTask;
    }
}
