CREATE DATABASE CountriesDB;
GO
USE CountriesDB;
GO

CREATE TABLE Regions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Countries (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    RegionId INT NOT NULL FOREIGN KEY REFERENCES Regions(Id),
    Area FLOAT NOT NULL,
    Population BIGINT NOT NULL
);

CREATE TABLE Cities (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    CountryId INT NOT NULL FOREIGN KEY REFERENCES Countries(Id),
    Population BIGINT NOT NULL,
    IsCapital BIT NOT NULL 
);
GO

INSERT INTO Regions (Name) VALUES (N'Европа'), (N'Азия');

INSERT INTO Countries (Name, RegionId, Area, Population) VALUES 
(N'Россия', 1, 17098242, 143400000),
(N'Франция', 1, 551695, 67000000),
(N'Китай', 2, 9596961, 1400000000),
(N'Дания', 1, 42951, 5800000);

INSERT INTO Cities (Name, CountryId, Population, IsCapital) VALUES 
(N'Москва', 1, 13000000, 1),
(N'Санкт-Петербург', 1, 5600000, 0),
(N'Новосибирск', 1, 1600000, 0),
(N'Екатеринбург', 1, 1500000, 0),
(N'Париж', 2, 2161000, 1),
(N'Марсель', 2, 861000, 0),
(N'Пекин', 3, 21542000, 1),
(N'Шанхай', 3, 24256800, 0),
(N'Копенгаген', 4, 602481, 1);
GO