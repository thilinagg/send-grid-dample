using SendGrid;

namespace SendGridEmailSample.Application.Interfaces;

public interface IEmailSenderService
{
    Task<Response> SendSingleAsync(string receiverEmail, string subject, string body);

    Task<Response> SendSingleTemplateEmailToMultipleRecipientsAsync(List<string> receiversEmails, string subject,
        string body);
}
