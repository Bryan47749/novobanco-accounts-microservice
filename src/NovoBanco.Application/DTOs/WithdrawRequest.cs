public class WithdrawRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Reference { get; set; } = null!;
}
