using Microsoft.EntityFrameworkCore;
using SendGridEmailSample.Domain.Entities;

namespace SendGridEmailSample.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<EmailAlert> EmailAlerts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
