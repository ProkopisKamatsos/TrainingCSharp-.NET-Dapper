-- ============================================================
-- BankingApiDB Database Script
-- 
-- Σημείωση για Dapper compatibility:
-- Χρησιμοποιούμε DATETIME2 παντού — όχι DATE ή DATETIME.
-- Ο λόγος: Dapper δεν υποστηρίζει DateOnly natively.
-- Αν χρησιμοποιήσεις DATE στη SQL, το Dapper θα προσπαθήσει
-- να το κάνει map σε DateTime και θα σπάσει.
-- Στα C# models θα χρησιμοποιούμε DateTime παντού.
-- ============================================================


USE BankingApiDB;
GO

-- ============================================================
-- CUSTOMERS
-- ============================================================
CREATE TABLE Customers (
    CustomerId   INT            NOT NULL IDENTITY(1,1),
    FirstName    NVARCHAR(50)   NOT NULL,
    LastName     NVARCHAR(50)   NOT NULL,
    Email        NVARCHAR(100)  NOT NULL,
    PasswordHash NVARCHAR(255)  NOT NULL,
    PhoneNumber  NVARCHAR(20)   NULL,
    IsActive     BIT            NOT NULL DEFAULT 1,
    CreatedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt    DATETIME2      NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_Customers PRIMARY KEY (CustomerId),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
GO

-- ============================================================
-- ACCOUNTS
-- ============================================================
CREATE TABLE Accounts (
    AccountId     INT            NOT NULL IDENTITY(1,1),
    CustomerId    INT            NOT NULL,
    AccountNumber NVARCHAR(20)   NOT NULL,
    AccountType   NVARCHAR(10)   NOT NULL,   -- 'Checking' | 'Savings'
    Balance       DECIMAL(18,2)  NOT NULL DEFAULT 0.00,
    IsActive      BIT            NOT NULL DEFAULT 1,
    CreatedAt     DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt     DATETIME2      NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_Accounts PRIMARY KEY (AccountId),
    CONSTRAINT UQ_Accounts_AccountNumber UNIQUE (AccountNumber),
    CONSTRAINT FK_Accounts_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId),
    CONSTRAINT CK_Accounts_Balance CHECK (Balance >= 0),
    CONSTRAINT CK_Accounts_Type CHECK (AccountType IN ('Checking', 'Savings'))
);
GO

-- ============================================================
-- TRANSACTIONS
-- Κανόνες:
--   Deposit    → FromAccountId IS NULL,     ToAccountId IS NOT NULL
--   Withdrawal → FromAccountId IS NOT NULL, ToAccountId IS NULL
--   Transfer   → FromAccountId IS NOT NULL, ToAccountId IS NOT NULL
--
-- Δεν υπάρχει UpdatedAt — transactions είναι immutable.
-- Αν κάτι πάει λάθος, καταγράφεται νέα εγγραφή με Status='Failed'.
-- ============================================================
CREATE TABLE Transactions (
    TransactionId   INT            NOT NULL IDENTITY(1,1),
    FromAccountId   INT            NULL,
    ToAccountId     INT            NULL,
    Amount          DECIMAL(18,2)  NOT NULL,
    TransactionType NVARCHAR(10)   NOT NULL, -- 'Deposit' | 'Withdrawal' | 'Transfer'
    Status          NVARCHAR(10)   NOT NULL DEFAULT 'Completed', -- 'Completed' | 'Failed'
    Description     NVARCHAR(255)  NULL,
    CreatedAt       DATETIME2      NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_Transactions PRIMARY KEY (TransactionId),
    CONSTRAINT FK_Transactions_FromAccount FOREIGN KEY (FromAccountId)
        REFERENCES Accounts(AccountId),
    CONSTRAINT FK_Transactions_ToAccount FOREIGN KEY (ToAccountId)
        REFERENCES Accounts(AccountId),
    CONSTRAINT CK_Transactions_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Transactions_Type CHECK (TransactionType IN ('Deposit', 'Withdrawal', 'Transfer')),
    CONSTRAINT CK_Transactions_Status CHECK (Status IN ('Completed', 'Failed')),
    CONSTRAINT CK_Transactions_Accounts CHECK (
        FromAccountId IS NOT NULL OR ToAccountId IS NOT NULL
    )
);
GO

-- ============================================================
-- INDEXES
-- ============================================================
CREATE INDEX IX_Accounts_CustomerId
    ON Accounts(CustomerId);

CREATE INDEX IX_Transactions_FromAccountId
    ON Transactions(FromAccountId);

CREATE INDEX IX_Transactions_ToAccountId
    ON Transactions(ToAccountId);

CREATE INDEX IX_Transactions_CreatedAt
    ON Transactions(CreatedAt DESC);
GO