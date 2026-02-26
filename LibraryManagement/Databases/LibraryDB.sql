CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    Category NVARCHAR(100),
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    AvailableQuantity INT NOT NULL CHECK (AvailableQuantity >= 0),
    CONSTRAINT CK_AvailableQuantity CHECK (AvailableQuantity <= Quantity)
);

CREATE TABLE Readers (
    ReaderId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(150)
);

CREATE TABLE BorrowRecords (
    BorrowId INT IDENTITY(1,1) PRIMARY KEY,
    ReaderId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT N'Borrowing',
    FOREIGN KEY (ReaderId) REFERENCES Readers(ReaderId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
);
