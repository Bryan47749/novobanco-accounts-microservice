using Microsoft.EntityFrameworkCore;
using NovoBanco.Domain.Entities;

namespace NovoBanco.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Transfer> Transfers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
