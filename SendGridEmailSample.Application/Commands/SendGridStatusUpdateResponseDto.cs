using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Commands;

public record SendGridStatusUpdateResponseDto(Guid Id, EmailStatus Status);
