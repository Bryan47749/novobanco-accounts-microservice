using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;
using NovoBanco.Domain.Entities;

namespace NovoBanco.Infrastructure.Persistence;

public class NovoBancoDbContext : DbContext, IApplicationDbContext
{
    public NovoBancoDbContext(DbContextOptions<NovoBancoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Transfer> Transfers => Set<Transfer>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NovoBancoDbContext).Assembly);
    }
}
