namespace NovoBanco.Domain.Enums;

public enum AccountStatus
{
    ACTIVE,
    BLOCKED,
    CLOSED
}

public enum AccountType
{
    SAVINGS = 0,
    CHECKING = 1
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
