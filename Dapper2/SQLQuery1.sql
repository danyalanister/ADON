CREATE DATABASE PromoMailingDb;
GO
USE PromoMailingDb;
GO

CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(150) NOT NULL,
    BirthDate DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    City NVARCHAR(100) NOT NULL
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE CustomerCategories (
    CustomerId INT REFERENCES Customers(Id),
    CategoryId INT REFERENCES Categories(Id),
    PRIMARY KEY (CustomerId, CategoryId)
);

CREATE TABLE Promotions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    CategoryId INT REFERENCES Categories(Id),
    Country NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);
GO

INSERT INTO Customers (FullName, BirthDate, Gender, Email, Country, City) VALUES
(N'Иванов Иван', '2000-05-15', N'М', 'ivanov@mail.com', N'Россия', N'Чита'),
(N'Смирнова Анна', '1995-10-20', N'Ж', 'smirnova@mail.com', N'Россия', N'Москва'),
(N'Джон Доу', '1990-01-10', N'М', 'john@mail.com', N'США', N'Нью-Йорк');

INSERT INTO Categories (Name) VALUES (N'Мобильные телефоны'), (N'Ноутбуки'), (N'Кухонная техника');

INSERT INTO CustomerCategories (CustomerId, CategoryId) VALUES (1, 1), (1, 2), (2, 3), (3, 1);

INSERT INTO Promotions (Name, CategoryId, Country, StartDate, EndDate) VALUES
(N'Скидка 20% на iPhone 15', 1, N'Россия', '2026-10-01', '2026-11-01'),
(N'Ноутбуки ASUS по суперцене', 2, N'Россия', '2026-10-15', '2026-11-15'),
(N'Black Friday Apple Deals', 1, N'США', '2026-11-20', '2026-11-30');
GO