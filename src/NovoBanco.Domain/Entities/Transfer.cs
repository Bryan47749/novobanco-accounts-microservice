using NovoBanco.Domain.Enums;

namespace NovoBanco.Domain.Entities;

public class Transfer
{
    public Guid Id { get; set; }

    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }

    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }

    public string Reference { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
