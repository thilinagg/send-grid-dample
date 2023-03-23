using MediatR;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Entities;
using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Commands;

public record EmailSendProcessorCommand(string ReceiverEmail, string Subject, string Body): IRequest;


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
        var response = await _emailSenderService.SendSingleAsync(request.ReceiverEmail, request.Subject, request.Body);
        var sendGridMsgId = string.Empty;
        if(response.IsSuccessStatusCode)
        {
            sendGridMsgId = response.Headers.SingleOrDefault(x => x.Key == "X-Message-Id").Value.SingleOrDefault();
        }

        var emailAlert = new EmailAlert();
        emailAlert.Create(
            request.ReceiverEmail, 
            request.Subject, 
            request.Body,
            sendGridMsgId,
            string.IsNullOrEmpty(sendGridMsgId)? EmailStatus.Failed: EmailStatus.Processed);

        await _context.EmailAlerts.AddAsync(emailAlert);
        await _context.SaveChangesAsync(cancellationToken);
        
    }
}
