using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGridEmailSample.Application.Interfaces;

namespace SendGridEmailSample.Application.Queries.EmailList;

public class GetAllEmailQueryHandler : IRequestHandler<GetAllEmailQuery, List<EmailAlertDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllEmailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmailAlertDto>> Handle(GetAllEmailQuery request, CancellationToken cancellationToken)
    {
        List<EmailAlertDto> emailList = await _context.EmailAlerts
            .OrderBy(x => x.CreatedDateTime)
            .Select(e => new EmailAlertDto(
                e.Id,
                e.ReceiverEmail,
                e.Subject,
                e.Body,
                e.Status,
                e.CreatedDateTime))
            .ToListAsync(cancellationToken);

        return emailList;
    }
}