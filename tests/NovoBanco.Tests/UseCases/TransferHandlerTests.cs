using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

namespace NovoBanco.Application.UseCases.Transfer
{

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

            var from = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);

            var to = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

            if (from == null || to == null)
                throw new Exception("Account not found");

            if (from.Status != AccountStatus.ACTIVE || to.Status != AccountStatus.ACTIVE)
                throw new Exception("One of the accounts is not active");

            if (from.Balance < request.Amount)
                throw new Exception("Insufficient balance");

            // Actualizar balances directamente, sin transacción
            from.Balance -= request.Amount;
            to.Balance += request.Amount;

            // Registrar la transferencia
            _context.Transfers.Add(new Domain.Entities.Transfer
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
        }
    }
}
