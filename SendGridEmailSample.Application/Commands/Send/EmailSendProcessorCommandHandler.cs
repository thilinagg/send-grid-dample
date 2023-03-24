using MediatR;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Entities;
using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Commands.Send;

public class EmailSendProcessorCommandHandler : IRequestHandler<EmailSendProcessorCommand>
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly IApplicationDbContext _context;

    public EmailSendProcessorCommandHandler(IEmailSenderService emailSenderService, IApplicationDbContext context)
    {
        _emailSenderService = emailSenderService;
        _context = context;
    }

    public async Task Handle(EmailSendProcessorCommand request, CancellationToken cancellationToken)
    {
        if (request.IsBulk)
            await SendBulkAsync(request);
        else
            await SendSingleAsync(request);
    }

    private async Task SendSingleAsync(EmailSendProcessorCommand request)
    {
        var response = await _emailSenderService.SendSingleAsync(request.ReceiverEmail, request.Subject, request.Body);
        var sendGridMsgId = string.Empty;
        if (response.IsSuccessStatusCode)
        {
            sendGridMsgId = response.Headers.SingleOrDefault(x => x.Key == "X-Message-Id").Value.SingleOrDefault();
        }

        await SaveEmailAlert(request.ReceiverEmail, request.Subject, request.Body, sendGridMsgId);
    }

    private async Task SendBulkAsync(EmailSendProcessorCommand request)
    {
        var toEmails = request.ReceiverEmail.Split(';').ToList();
        var response = await _emailSenderService
            .SendSingleTemplateEmailToMultipleRecipientsAsync(toEmails, request.Subject, request.Body);
        var sendGridMsgId = string.Empty;
        if (response.IsSuccessStatusCode)
        {
            sendGridMsgId = response.Headers.SingleOrDefault(x => x.Key == "X-Message-Id").Value.SingleOrDefault();
        }

        foreach (var toEmail in toEmails)
        {
            await SaveEmailAlert(toEmail, request.Subject, request.Body, sendGridMsgId);
        }
    }

    private async Task SaveEmailAlert(string toEmail, string subject, string body, string? sendGridMsgId)
    {
        var emailAlert = new EmailAlert();
        emailAlert.Create(
            toEmail,
            subject,
            body,
            sendGridMsgId,
            string.IsNullOrEmpty(sendGridMsgId) ? EmailStatus.Failed : EmailStatus.Processed);

        await _context.EmailAlerts.AddAsync(emailAlert);
        await _context.SaveChangesAsync();
    }
}
