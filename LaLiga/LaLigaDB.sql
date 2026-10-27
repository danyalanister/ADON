CREATE DATABASE LaLigaDb;
GO
USE LaLigaDb;
GO

CREATE TABLE Teams (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100) NOT NULL);
CREATE TABLE Players (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(100) NOT NULL, TeamId INT REFERENCES Teams(Id), Goals INT NOT NULL);
CREATE TABLE Matches (Id INT IDENTITY PRIMARY KEY, HomeTeamId INT REFERENCES Teams(Id), AwayTeamId INT REFERENCES Teams(Id), HomeGoals INT, AwayGoals INT);
GO

INSERT INTO Teams (Name) VALUES (N'Реал Мадрид'), (N'Барселона'), (N'Атлетико Мадрид'), (N'Севилья');
INSERT INTO Players (Name, TeamId, Goals) VALUES 
(N'Винисиус', 1, 15), (N'Беллингем', 1, 18), (N'Левандовски', 2, 20), (N'Педри', 2, 5),
(N'Гризманн', 3, 16), (N'Депай', 3, 8), (N'Рамос', 4, 3), (N'Эн-Несири', 4, 12);
GO

CREATE PROCEDURE sp_GenerateRandomMatches
AS
BEGIN
    DELETE FROM Matches;
    INSERT INTO Matches (HomeTeamId, AwayTeamId, HomeGoals, AwayGoals)
    SELECT t1.Id, t2.Id, ABS(CHECKSUM(NEWID())) % 5, ABS(CHECKSUM(NEWID())) % 4
    FROM Teams t1 CROSS JOIN Teams t2 WHERE t1.Id <> t2.Id;
END;
GO

CREATE VIEW vw_TeamStats AS
WITH MatchData AS (
    SELECT HomeTeamId AS TeamId, HomeGoals AS Scored, AwayGoals AS Conceded,
           CASE WHEN HomeGoals > AwayGoals THEN 3 WHEN HomeGoals = AwayGoals THEN 1 ELSE 0 END AS Points
    FROM Matches
    UNION ALL
    SELECT AwayTeamId, AwayGoals, HomeGoals,
           CASE WHEN AwayGoals > HomeGoals THEN 3 WHEN AwayGoals = HomeGoals THEN 1 ELSE 0 END
    FROM Matches
)
SELECT t.Id AS TeamId, t.Name AS TeamName,
       ISNULL(SUM(m.Scored), 0) AS GoalsScored,
       ISNULL(SUM(m.Conceded), 0) AS GoalsConceded,
       ISNULL(SUM(m.Points), 0) AS TotalPoints
FROM Teams t
LEFT JOIN MatchData m ON t.Id = m.TeamId
GROUP BY t.Id, t.Name;
GO