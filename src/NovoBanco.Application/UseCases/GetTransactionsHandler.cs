using Microsoft.EntityFrameworkCore;
using NovoBanco.Application.Interfaces;

public class GetTransactionsHandler
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(Guid accountId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Transactions
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt);

        var total = await query.CountAsync(ct);

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new
        {
            total,
            page,
            pageSize,
            data
        };
    }
}
