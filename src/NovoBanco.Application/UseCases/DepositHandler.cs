using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

namespace NovoBanco.Application.UseCases.Deposit;

public class DepositHandler
{
    private readonly IApplicationDbContext _context;

    public DepositHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DepositRequest request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            throw new Exception("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(request.Reference))
            throw new Exception("Reference is required");

        // Idempotencia
        var exists = await _context.Transactions
            .AnyAsync(x => x.Reference == request.Reference, cancellationToken);

        if (exists)
            throw new Exception("Duplicate transaction");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

        if (account == null)
            throw new Exception("Account not found");

        if (account.Status != AccountStatus.ACTIVE)
            throw new Exception("Account is not active");

        account.Balance += request.Amount;

        _context.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = request.Amount,
            Type = TransactionType.DEPOSIT,
            Status = TransactionStatus.SUCCESS,
            Reference = request.Reference,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}
