using MediatR;

namespace SendGridEmailSample.Application.Commands.Send;

public record EmailSendProcessorCommand(
    string ReceiverEmail,
    string Subject, 
    string Body, 
    bool IsBulk) : IRequest;