using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using LaLigaApp; 

namespace LaLigaTests
{
    public class EfCoreTests
    {
        [Fact]
        public void TopScorer_Returns_PlayerWithMostGoals() 
        {
            
            var options = new DbContextOptionsBuilder<LaLigaContext>()
                .UseInMemoryDatabase(databaseName: "TestLaLigaDb")
                .Options;

            using (var db = new LaLigaContext(options))
            {
                
                db.Players.RemoveRange(db.Players);

                // Добавляем тестовые данные
                db.Players.Add(new Player { Name = "Слабый игрок", Goals = 2, TeamId = 1 });
                db.Players.Add(new Player { Name = "Крутой бомбардир", Goals = 30, TeamId = 1 });
                db.SaveChanges();
            }

            using (var db = new LaLigaContext(options))
            {
                
                db.Players.RemoveRange(db.Players);
                db.Teams.RemoveRange(db.Teams);

                
                db.Teams.Add(new Team { Id = 1, Name = "Тестовая Команда" });

                
                db.Players.Add(new Player { Name = "Слабый игрок", Goals = 2, TeamId = 1 });
                db.Players.Add(new Player { Name = "Крутой бомбардир", Goals = 30, TeamId = 1 });
                db.SaveChanges();
            }
        }
    }
}