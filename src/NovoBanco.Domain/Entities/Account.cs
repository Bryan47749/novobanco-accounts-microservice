using System.Text.Json.Serialization;
using NovoBanco.Domain.Enums;

namespace NovoBanco.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = null!;
    public Guid CustomerId { get; set; }

    public AccountType Type { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Balance { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AccountStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
