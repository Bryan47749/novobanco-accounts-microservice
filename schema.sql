-- =========================================
-- EXTENSIONES
-- =========================================
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- =========================================
-- ENUMS
-- =========================================
CREATE TYPE account_status AS ENUM ('ACTIVE', 'BLOCKED', 'CLOSED');
CREATE TYPE account_type AS ENUM ('SAVINGS', 'CHECKING');

CREATE TYPE transaction_type AS ENUM ('DEPOSIT', 'WITHDRAWAL', 'TRANSFER');
CREATE TYPE transaction_status AS ENUM ('SUCCESS', 'FAILED', 'REVERSED');

-- =========================================
-- TABLA: customers
-- =========================================
CREATE TABLE customers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    identification VARCHAR(15) NOT NULL UNIQUE,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,

    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_customers_identification ON customers(identification);

-- =========================================
-- TABLA: accounts
-- =========================================
CREATE TABLE accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    account_number VARCHAR(20) NOT NULL UNIQUE,

    customer_id UUID NOT NULL,
    CONSTRAINT fk_accounts_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    type account_type NOT NULL,
    currency VARCHAR(3) NOT NULL DEFAULT 'USD',

    balance NUMERIC(18,2) NOT NULL DEFAULT 0,

    status account_status NOT NULL DEFAULT 'ACTIVE',

    created_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_balance_non_negative CHECK (balance >= 0)
);

CREATE INDEX idx_accounts_customer_id ON accounts(customer_id);
CREATE INDEX idx_accounts_status ON accounts(status);

-- =========================================
-- TABLA: transactions
-- =========================================
CREATE TABLE transactions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    account_id UUID NOT NULL,
    CONSTRAINT fk_transactions_account
        FOREIGN KEY (account_id) REFERENCES accounts(id),

    amount NUMERIC(18,2) NOT NULL CHECK (amount > 0),

    type transaction_type NOT NULL,

    status transaction_status NOT NULL,

    reference VARCHAR(100) NOT NULL UNIQUE,

    description TEXT,

    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_transactions_account_date
ON transactions(account_id, created_at DESC);

-- =========================================
-- TABLA: transfers
-- =========================================
CREATE TABLE transfers (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),

    from_account_id UUID NOT NULL,
    to_account_id UUID NOT NULL,

    CONSTRAINT fk_transfer_from_account
        FOREIGN KEY (from_account_id) REFERENCES accounts(id),

    CONSTRAINT fk_transfer_to_account
        FOREIGN KEY (to_account_id) REFERENCES accounts(id),

    amount NUMERIC(18,2) NOT NULL CHECK (amount > 0),

    status transaction_status NOT NULL,

    reference VARCHAR(100) NOT NULL UNIQUE,

    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_transfers_from_account ON transfers(from_account_id);
CREATE INDEX idx_transfers_to_account ON transfers(to_account_id);
