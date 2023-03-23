using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Enums;
using System.Text.Json.Serialization;

namespace SendGridEmailSample.Application.Commands;

public class SendGridStatusUpdateCommand: IRequest
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

public class SendGridStatusUpdateCommandHandler : IRequestHandler<SendGridStatusUpdateCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SendGridStatusUpdateCommandHandler> _logger;

    public SendGridStatusUpdateCommandHandler(IApplicationDbContext context, ILogger<SendGridStatusUpdateCommandHandler> logger)
    {
        _context = context;
       _logger = logger;
    }

    public async Task Handle(SendGridStatusUpdateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Event Email: {email}", request.Email);
        _logger.LogWarning("Event EventStatus: {email}", request.EventStatus);
        _logger.LogWarning("Event EventId: {email}", request.EventId);
        _logger.LogWarning("Event MessageId: {email}", request.MessageId);
        _logger.LogWarning("Event SendGridMessageId: {email}", request.SendGridMessageId);

        var isValidEvent = Enum.TryParse(request.EventStatus, true, out EmailStatus deliveryStatus);
        if (!isValidEvent)
            return;

        var emailAlert = await _context.EmailAlerts
            .FirstOrDefaultAsync(e => e.SendGridMessageId == request.SendGridMessageId);

        if (emailAlert is null)
            return;

        emailAlert.UpdateStatus(deliveryStatus);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
