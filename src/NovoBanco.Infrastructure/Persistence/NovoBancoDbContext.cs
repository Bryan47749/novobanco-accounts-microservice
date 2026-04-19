using Microsoft.EntityFrameworkCore;
using NovoBanco.Domain.Entities;

namespace NovoBanco.Infrastructure.Persistence;

public class NovoBancoDbContext : DbContext
{
    public NovoBancoDbContext(DbContextOptions<NovoBancoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Transfer> Transfers => Set<Transfer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NovoBancoDbContext).Assembly);
    }
}
