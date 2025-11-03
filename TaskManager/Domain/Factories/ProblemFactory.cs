using TaskManager.Domain.DTOs.ProblemDetails;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Factories;

public static class ProblemFactory
{
    public static ProblemDetailsDto NotFound(string detail, string instance) =>
        new ProblemDetailsDto(
            Type: EProblemType.NotFound,
            Title: "Resource not found",
            Status: 404,
            Detail: detail,
            Instance: instance,
            Timestamp: DateTimeOffset.UtcNow
        );

    public static ProblemDetailsDto ValidationError(string detail, string instance) =>
        new ProblemDetailsDto(
            Type: EProblemType.ValidationError,
            Title: "Validation error",
            Status: 400,
            Detail: detail,
            Instance: instance,
            Timestamp: DateTimeOffset.UtcNow
        );
}