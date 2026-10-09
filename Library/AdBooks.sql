USE Library;
GO

-- Очищаем таблицы
DELETE FROM Books;
DELETE FROM Authors;
GO

-- Временно разрешаем ручную вставку ID и жестко задаем автору номер 1
SET IDENTITY_INSERT Authors ON;
INSERT INTO Authors (Id, FirstName, LastName) 
VALUES (1, N'Роджер', N'Желязны');
SET IDENTITY_INSERT Authors OFF;
GO

-- Добавляем книги к автору с ID = 1
INSERT INTO Books (Title, AuthorId, Price, Pages)
VALUES
(N'Хроники Амбера', 1, 1500, 800),
(N'Ночь в тоскливом октябре', 1, 800, 320),
(N'Бог Света', 1, 1200, 450);
GO