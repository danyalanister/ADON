-- Создаем базу данных Library
CREATE DATABASE Library;
GO
USE Library;
GO

-- Создаем таблицу Авторов
CREATE TABLE [dbo].[Authors] (
    Id        INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName  VARCHAR(100) NOT NULL
);
GO

-- Создаем таблицу Книг
CREATE TABLE [dbo].[Books] (
    Id       INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Title    VARCHAR(200) NOT NULL,
    AuthorId INT NOT NULL,
    Price    INT NOT NULL,
    Pages    INT NOT NULL,
    FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);
GO

-- Создаем хранимую процедуру для подсчета книг автора
CREATE PROCEDURE getBooksNumber
    @AuthorId int,
    @BookCount int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @BookCount = COUNT(b.Id)
    FROM Books b
    INNER JOIN Authors a ON b.AuthorId = a.Id
    WHERE a.Id = @AuthorId;
END;
GO