using MediatR;

namespace SendGridEmailSample.Application.Queries.EmailList;

public record GetAllEmailQuery() : IRequest<List<EmailAlertDto>>;