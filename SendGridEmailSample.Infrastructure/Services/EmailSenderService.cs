
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Infrastructure.Configs;

namespace SendGridEmailSample.Infrastructure.Services;

public sealed class EmailSenderService : IEmailSenderService
{
    private readonly SendGridConfigs _sendGridConfigs;

    public EmailSenderService(IOptionsSnapshot<SendGridConfigs> sendGridConfigs)
    {
        _sendGridConfigs = sendGridConfigs.Value;
    }

    public async Task<Response> SendSingleAsync(string receiverEmail, string subject, string body)
    {
        var client = new SendGridClient(_sendGridConfigs.ApiKey);
        var dynamicDataObject = new
        {
            email_subject = subject,
            email_body = body,
            send_date = DateTime.Now.ToString("dd/MM/yyyy"),
        };
        var msg = MailHelper.CreateSingleTemplateEmail(
            new EmailAddress(_sendGridConfigs.SenderEmail, _sendGridConfigs.SenderName),
            new EmailAddress(receiverEmail),
            _sendGridConfigs.TemplateId,
            dynamicDataObject);

        return await client.SendEmailAsync(msg);
    }

    public async Task<Response> SendSingleTemplateEmailToMultipleRecipientsAsync(List<string> receiversEmails, string subject, string body)
    {
        var client = new SendGridClient(_sendGridConfigs.ApiKey);
        var dynamicDataObject = new
        {
            email_subject = subject,
            email_body = body,
            send_date = DateTime.Now.ToString("dd/MM/yyyy"),
        };

        var tos = new List<EmailAddress>();
        foreach (var email in receiversEmails)
        {
            tos.Add(new EmailAddress(email));
        }

        var msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(
            new EmailAddress(_sendGridConfigs.SenderEmail, _sendGridConfigs.SenderName),
            tos,
            _sendGridConfigs.TemplateId,
            dynamicDataObject);

        return await client.SendEmailAsync(msg);
    }
}
