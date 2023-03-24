using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Domain.Entities;

public class EmailAlert : BaseEntity
{
    public string ReceiverEmail { get; private set; } = default!;
    public string Subject { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public string? SendGridMessageId { get; private set; }
    public EmailStatus Status { get; private set; }

    public void Create(
        string receiverEmail,
        string subject,
        string body,
        string? sendGridMessageId,
        EmailStatus status)
    {
        ReceiverEmail = receiverEmail;
        Subject = subject;
        Body = body;
        SendGridMessageId = sendGridMessageId;
        Status = status;
    }

    public void UpdateStatus(EmailStatus status)
        => Status = status;

}
