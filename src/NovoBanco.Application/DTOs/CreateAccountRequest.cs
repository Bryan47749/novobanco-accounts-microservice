public class CreateAccountRequest
{
    public Guid CustomerId { get; set; }
    public string Type { get; set; } = null!;
}
