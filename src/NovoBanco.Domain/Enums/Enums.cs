namespace NovoBanco.Domain.Enums;

public enum AccountStatus
{
    ACTIVE,
    BLOCKED,
    CLOSED
}

public enum AccountType
{
    SAVINGS,
    CHECKING
}

public enum TransactionType
{
    DEPOSIT,
    WITHDRAWAL,
    TRANSFER
}

public enum TransactionStatus
{
    SUCCESS,
    FAILED,
    REVERSED
}
