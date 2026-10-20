CREATE DATABASE StudentGrades1;
GO
USE StudentGrades1;
GO
CREATE TABLE Grades (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentName NVARCHAR(100),
    Subject NVARCHAR(50),
    Score INT
);
INSERT INTO Grades (StudentName, Subject, Score) VALUES 
(N'Иванов Иван', N'Математика', 5),
(N'Петров Петр', N'Физика', 4),
(N'Сидорова Анна', N'Информатика', 5);
GO

CREATE DATABASE StudentGrades2;
GO
USE StudentGrades2;
GO
CREATE TABLE Grades (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StudentName NVARCHAR(100),
    Subject NVARCHAR(50),
    Score INT
);
INSERT INTO Grades (StudentName, Subject, Score) VALUES 
(N'Смирнов Алексей', N'История', 3),
(N'Козлова Мария', N'Химия', 5),
(N'Волков Дмитрий', N'Биология', 4);
GO