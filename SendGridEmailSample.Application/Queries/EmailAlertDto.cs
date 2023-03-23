using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Queries;

public record EmailAlertDto(
    Guid Id,
    string ReceiverEmail, 
    string Subject, 
    string Body,
    EmailStatus Status,
    DateTime CreatedDateTime);
