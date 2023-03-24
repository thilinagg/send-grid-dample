using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Enums;

namespace SendGridEmailSample.Application.Commands.UpdateStatus;

public class SendGridStatusUpdateCommand: IRequest<SendGridStatusUpdateResponseDto?>
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("event")]
    public string EventStatus { get; set; } = default!;

    [JsonPropertyName("sg_event_id")]
    public string EventId { get; set; } = default!;

    [JsonPropertyName("sg_message_id")]
    public string MessageId { get; set; } = default!;

    public string SendGridMessageId => MessageId.Split('.')[0];
}

public class
    SendGridStatusUpdateCommandHandler : IRequestHandler<SendGridStatusUpdateCommand, SendGridStatusUpdateResponseDto?>
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
            .FirstOrDefaultAsync(e => e.SendGridMessageId == request.SendGridMessageId);

        if (emailAlert is null)
            return null;

        emailAlert.UpdateStatus(deliveryStatus);
        await _context.SaveChangesAsync(cancellationToken);

        return new SendGridStatusUpdateResponseDto(emailAlert.Id, emailAlert.Status);
    }
}
