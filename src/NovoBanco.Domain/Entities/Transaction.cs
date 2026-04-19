using NovoBanco.Domain.Enums;

namespace NovoBanco.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }

    public string Reference { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public Account Account { get; set; } = null!;
}
