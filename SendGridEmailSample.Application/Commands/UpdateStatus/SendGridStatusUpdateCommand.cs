using MediatR;
using System.Text.Json.Serialization;

namespace SendGridEmailSample.Application.Commands.UpdateStatus;

public class SendGridStatusUpdateCommand : IRequest<SendGridStatusUpdateResponseDto?>
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