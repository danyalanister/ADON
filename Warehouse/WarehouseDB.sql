CREATE DATABASE WarehouseDB;
GO
USE WarehouseDB;
GO

-- Создаем таблицу типов товаров
CREATE TABLE ProductTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL
);

-- Создаем таблицу поставщиков
CREATE TABLE Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL
);

-- Создаем главную таблицу товаров
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(200) NOT NULL,
    TypeId INT NOT NULL FOREIGN KEY REFERENCES ProductTypes(Id),
    SupplierId INT NOT NULL FOREIGN KEY REFERENCES Suppliers(Id),
    Quantity INT NOT NULL,
    CostPrice DECIMAL(18,2) NOT NULL,
    SupplyDate DATE NOT NULL
);
GO

-- Заполняем тестовыми данными для проверки всех заданий
INSERT INTO ProductTypes (TypeName) VALUES (N'Электроника'), (N'Мебель'), (N'Продукты');
INSERT INTO Suppliers (SupplierName) VALUES (N'ООО ТехПром'), (N'ЗАО МебельОпт'), (N'ИП Вкусняшка');

INSERT INTO Products (ProductName, TypeId, SupplierId, Quantity, CostPrice, SupplyDate) VALUES
(N'Ноутбук', 1, 1, 50, 45000.00, '2026-09-01'),
(N'Смартфон', 1, 1, 150, 20000.00, '2026-09-15'),
(N'Стол офисный', 2, 2, 30, 5000.00, '2026-08-20'),
(N'Кресло', 2, 2, 45, 3500.00, '2026-08-25'),
(N'Печенье', 3, 3, 500, 150.00, '2026-10-01');
GO