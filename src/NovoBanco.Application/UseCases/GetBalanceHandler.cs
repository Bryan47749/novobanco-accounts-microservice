using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;

public class GetBalanceHandler
{
    private readonly IApplicationDbContext _context;

    public GetBalanceHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(string accountNumber, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber, cancellationToken);

        if (account == null)
            throw new Exception("Account not found");

        return new
        {
            account.Id,
            account.AccountNumber,
            account.Balance,
            account.Status
            
        };
    }
}
