using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

public class CreateAccountHandler
{
    private readonly IApplicationDbContext _context;

    public CreateAccountHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (customer == null)
            throw new Exception("Customer not found");

        var account = new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            AccountNumber = GenerateAccountNumber(),
            Type = Enum.Parse<AccountType>(request.Type),
            Balance = 0,
            Status = AccountStatus.ACTIVE,
            Currency = "USD",
            CreatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.Id;
    }

    private string GenerateAccountNumber()
    {
        return DateTime.UtcNow.Ticks.ToString();
    }
}
