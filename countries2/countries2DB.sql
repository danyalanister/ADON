CREATE DATABASE CountriesDB;
GO
USE CountriesDB;
GO

-- 1. Создаем таблицы
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

-- 2. Заполняем базовыми данными
INSERT INTO Regions (Name) VALUES (N'Европа'), (N'Азия');

INSERT INTO Countries (Name, RegionId, Area, Population) VALUES 
(N'Россия', 1, 17098242, 143400000),
(N'Франция', 1, 551695, 67000000),
(N'Китай', 2, 9596961, 1400000000),
(N'Дания', 1, 42951, 5800000);

INSERT INTO Cities (Name, CountryId, Population, IsCapital) VALUES 
(N'Москва', 1, 13000000, 1),
(N'Санкт-Петербург', 1, 5600000, 0),
(N'Париж', 2, 2161000, 1),
(N'Пекин', 3, 21542000, 1),
(N'Копенгаген', 4, 602481, 1);
GO

-- 3. Создаем хранимые процедуры для Задания 4
CREATE PROCEDURE sp_GetTop3Countries AS
BEGIN SELECT TOP 3 * FROM Countries ORDER BY Population DESC; END;
GO

CREATE PROCEDURE sp_GetTop3Capitals AS
BEGIN SELECT TOP 3 * FROM Cities WHERE IsCapital = 1 ORDER BY Population DESC; END;
GO

CREATE PROCEDURE sp_GetMaxPopulationCountry AS
BEGIN SELECT TOP 1 * FROM Countries ORDER BY Population DESC; END;
GO

CREATE PROCEDURE sp_GetMaxPopulationCity AS
BEGIN SELECT TOP 1 * FROM Cities ORDER BY Population DESC; END;
GO