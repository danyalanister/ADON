-- Основная база данных
CREATE DATABASE VegsAndFruits;
GO
USE VegsAndFruits;
GO
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Type NVARCHAR(20),
    Color NVARCHAR(20),
    Calories INT
);
INSERT INTO Products (Name, Type, Color, Calories) VALUES 
(N'Яблоко', N'Фрукт', N'Красный', 52),
(N'Огурец', N'Овощ', N'Зеленый', 15),
(N'Банан', N'Фрукт', N'Желтый', 89);
GO

-- Запасная база данных (для имитации смены СУБД)
CREATE DATABASE VegsAndFruits2;
GO
USE VegsAndFruits2;
GO
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Type NVARCHAR(20),
    Color NVARCHAR(20),
    Calories INT
);
INSERT INTO Products (Name, Type, Color, Calories) VALUES 
(N'Помидор', N'Овощ', N'Красный', 18),
(N'Груша', N'Фрукт', N'Желтый', 57),
(N'Картофель', N'Овощ', N'Коричневый', 77);
GO