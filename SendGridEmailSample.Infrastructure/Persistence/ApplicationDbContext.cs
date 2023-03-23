using Microsoft.EntityFrameworkCore;
using SendGridEmailSample.Application.Interfaces;
using SendGridEmailSample.Domain.Entities;
using SendGridEmailSample.Infrastructure.Persistence.Interceptors;

namespace SendGridEmailSample.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<EmailAlert> EmailAlerts => Set<EmailAlert>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
