using FluentResults;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api70.Application.PipelineBehaviors;
public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly IValidator<TRequest>[] validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = (validators ?? Array.Empty<IValidator<TRequest>>()).ToArray();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationFailures = validators
            .Select(validator => validator.Validate(request))
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null)
            .Select(validationFailure => new Error(validationFailure.ErrorMessage))
            .ToArray();

        if (!validationFailures.Any())
            return await next().ConfigureAwait(false);

        var result = new TResponse();
        result.Reasons.AddRange(validationFailures);
        return result;
    }
}
