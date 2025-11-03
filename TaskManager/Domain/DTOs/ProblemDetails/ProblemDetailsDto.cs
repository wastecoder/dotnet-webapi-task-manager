using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs.ProblemDetails;

public record ProblemDetailsDto(
    EProblemType Type,
    string Title,
    int Status,
    string Detail,
    string Instance,
    DateTimeOffset Timestamp
);