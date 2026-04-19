using NovoBanco.Domain.Entities;

namespace NovoBanco.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Identification { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
