CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

CREATE TABLE Authors (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100)
);

CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200),
    PublishedYear INT,
    AuthorId INT FOREIGN KEY REFERENCES Authors(Id)
);

CREATE TABLE Members (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100)
);

CREATE TABLE BookCheckouts (
    Id INT PRIMARY KEY IDENTITY,
    BookId INT FOREIGN KEY REFERENCES Books(Id),
    MemberId INT FOREIGN KEY REFERENCES Members(Id),
    CheckoutDate DATE,
    ReturnDate DATE
);
GO


CREATE PROCEDURE GetBooksByAuthorId
    @AuthorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.Id,
        b.Title,
        b.PublishedYear,
        a.Name AS AuthorName
    FROM Books b
    INNER JOIN Authors a ON b.AuthorId = a.Id
    WHERE a.Id = @AuthorId
    ORDER BY b.Title;
END;
GO