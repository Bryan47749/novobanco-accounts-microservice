using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

namespace NovoBanco.Application.UseCases.Transfer;

public class TransferHandler
{
    private readonly IApplicationDbContext _context;
    public TransferHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(TransferRequest request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            throw new Exception("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(request.Reference))
            throw new Exception("Reference is required");

        // Idempotencia
        var exists = await _context.Transfers
            .AnyAsync(x => x.Reference == request.Reference, cancellationToken);

        if (exists)
            throw new Exception("Duplicate transfer");

        using var trx = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var from = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);

            var to = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

            if (from == null || to == null)
                throw new Exception("Account not found");

            if (from.Status != AccountStatus.ACTIVE || to.Status != AccountStatus.ACTIVE)
                throw new Exception("Invalid account status");

            if (from.Balance < request.Amount)
                throw new Exception("Insufficient balance");

            // Débito
            from.Balance -= request.Amount;

            // Crédito
            to.Balance += request.Amount;

            // Transacciones
            _context.Transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = from.Id,
                Amount = request.Amount,
                Type = TransactionType.TRANSFER,
                Status = TransactionStatus.SUCCESS,
                Reference = request.Reference + "-OUT",
                CreatedAt = DateTime.UtcNow
            });

            _context.Transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = to.Id,
                Amount = request.Amount,
                Type = TransactionType.TRANSFER,
                Status = TransactionStatus.SUCCESS,
                Reference = request.Reference + "-IN",
                CreatedAt = DateTime.UtcNow
            });

            // Transfer record
            _context.Transfers.Add(new NovoBanco.Domain.Entities.Transfer
            {
                Id = Guid.NewGuid(),
                FromAccountId = from.Id,
                ToAccountId = to.Id,
                Amount = request.Amount,
                Status = TransactionStatus.SUCCESS,
                Reference = request.Reference,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
            await trx.CommitAsync(cancellationToken);
        }
        catch
        {
            await trx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
