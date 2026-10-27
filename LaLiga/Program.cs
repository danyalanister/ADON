using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LaLigaApp
{
    // МОДЕЛИ И КОНТЕКСТ 
    public class Team { public int Id { get; set; } public string Name { get; set; } }
    public class Player { public int Id { get; set; } public string Name { get; set; } public int TeamId { get; set; } public int Goals { get; set; } public Team Team { get; set; } }

    public class TeamStat { public int TeamId { get; set; } public string TeamName { get; set; } public int GoalsScored { get; set; } public int GoalsConceded { get; set; } public int TotalPoints { get; set; } }

    public class LaLigaContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TeamStat> TeamStats { get; set; }

        public LaLigaContext() { }
        public LaLigaContext(DbContextOptions<LaLigaContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=LaLigaDb;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamStat>().HasNoKey().ToView("vw_TeamStats");
        }
    }

    // ОСНОВНАЯ ПРОГРАММА
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            using var db = new LaLigaContext();

            // ЗАДАНИЕ 1: Вызов хранимой процедуры для генерации матчей
            Console.WriteLine("ЗАДАНИЕ 1: Генерация матчей");
            db.Database.ExecuteSqlRaw("EXEC sp_GenerateRandomMatches");
            Console.WriteLine("Матчи успешно сгенерированы случайным образом!\n");

            // ЗАДАНИЕ 2: Бомбардиры
            Console.WriteLine("ЗАДАНИЕ 2: Статистика игроков");
            var bestTeamScorers = db.Players.Where(p => p.TeamId == 1).OrderByDescending(p => p.Goals).Take(3).ToList();
            Console.WriteLine("Топ-3 бомбардиров 'Реал Мадрид': " + string.Join(", ", bestTeamScorers.Select(p => $"{p.Name} ({p.Goals})")));
            Console.WriteLine("Лучший бомбардир 'Реал Мадрид': " + bestTeamScorers.First().Name);

            var bestAllScorers = db.Players.OrderByDescending(p => p.Goals).Take(3).ToList();
            Console.WriteLine("Топ-3 бомбардиров чемпионата: " + string.Join(", ", bestAllScorers.Select(p => $"{p.Name} ({p.Goals})")));
            Console.WriteLine("Лучший бомбардир чемпионата: " + bestAllScorers.First().Name + "\n");

            // Читаем данные из представления (View) для следующих заданий
            var stats = db.TeamStats.ToList();

            // ЗАДАНИЕ 3: Голы команд
            Console.WriteLine("ЗАДАНИЕ 3: Статистика голов");
            var topScoring = stats.OrderByDescending(s => s.GoalsScored).Take(3).ToList();
            Console.WriteLine("Топ-3 забивающих: " + string.Join(", ", topScoring.Select(s => $"{s.TeamName} ({s.GoalsScored})")));
            Console.WriteLine("Больше всего забила: " + topScoring.First().TeamName);

            var topDefending = stats.OrderBy(s => s.GoalsConceded).Take(3).ToList();
            Console.WriteLine("Топ-3 мало пропустивших: " + string.Join(", ", topDefending.Select(s => $"{s.TeamName} ({s.GoalsConceded})")));
            Console.WriteLine("Меньше всего пропустила: " + topDefending.First().TeamName + "\n");

            // ЗАДАНИЕ 4: Очки команд (3 за победу, 1 за ничью)
            Console.WriteLine("ЗАДАНИЕ 4: Турнирные очки");
            var topPoints = stats.OrderByDescending(s => s.TotalPoints).Take(3).ToList();
            Console.WriteLine("Топ-3 по очкам: " + string.Join(", ", topPoints.Select(s => $"{s.TeamName} ({s.TotalPoints})")));
            Console.WriteLine("Чемпион (максимум очков): " + topPoints.First().TeamName);

            var worstPoints = stats.OrderBy(s => s.TotalPoints).Take(3).ToList();
            Console.WriteLine("Топ-3 худших по очкам: " + string.Join(", ", worstPoints.Select(s => $"{s.TeamName} ({s.TotalPoints})")));
            Console.WriteLine("Аутсайдер (минимум очков): " + worstPoints.First().TeamName + "\n");

            Console.ReadLine();
        }
    }
}