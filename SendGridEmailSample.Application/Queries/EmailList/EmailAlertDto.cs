using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Queries.EmailList;

public record EmailAlertDto(
    Guid Id,
    string ReceiverEmail,
    string Subject,
    string Body,
    EmailStatus Status,
    DateTime CreatedDateTime);
