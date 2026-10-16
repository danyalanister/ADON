CREATE DATABASE Library;
GO
USE Library;
GO

CREATE TABLE Authors (
    Id INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50)
);

INSERT INTO Authors (Id, FirstName, LastName) VALUES
(1, N'Isaac', N'Asimov'),
(2, N'Ray', N'Bradbury'),
(3, N'Clifford', N'Simak'),
(1002, N'Roger', N'Zelazny');
GO

CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100),
    AuthorId INT,
    Price INT,
    Pages INT
);

INSERT INTO Books (Title, AuthorId, Price, Pages) VALUES
(N'I Robot', 1, 178, 358),
(N'The Door into Summer', 2, 220, 410),
(N'The Robots of Dawn', 1, 250, 480),
(N'The Goblin Reservation', 3, 230, 450);
GO