using FluentValidation.Results;

namespace MicroserviceProject.Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }
    
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .Select(failure => failure.ErrorMessage)
            .ToList();
    }

    public List<string> Errors { get; }
}