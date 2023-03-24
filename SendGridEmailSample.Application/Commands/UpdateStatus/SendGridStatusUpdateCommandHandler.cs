using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Commands.UpdateStatus;

public class SendGridStatusUpdateCommandHandler : IRequestHandler<SendGridStatusUpdateCommand, SendGridStatusUpdateResponseDto?>
{
    private readonly IApplicationDbContext _context;

    public SendGridStatusUpdateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SendGridStatusUpdateResponseDto?> Handle(SendGridStatusUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var isValidEvent = Enum.TryParse(request.EventStatus, true, out EmailStatus deliveryStatus);
        if (!isValidEvent)
            return null;

        var emailAlert = await _context.EmailAlerts
            .FirstOrDefaultAsync(e => 
                e.SendGridMessageId == request.SendGridMessageId 
                && e.ReceiverEmail == request.Email);

        if (emailAlert is null)
            return null;

        emailAlert.UpdateStatus(deliveryStatus);
        await _context.SaveChangesAsync(cancellationToken);

        return new SendGridStatusUpdateResponseDto(emailAlert.Id, emailAlert.Status);
    }
}
