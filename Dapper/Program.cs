using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient; 

namespace DapperExample
{
    // Класс-модель (POCO). 
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        // Строка подключения к локальной БД TestDB[cite: 29]
        static string connectionString = "Server=localhost;Database=TestDB;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("ПРАКТИКА: ИЗУЧЕНИЕ DAPPER\n");

            // 1. 
            var newUser = new User { Name = "Иван Петров", Email = "ivan@example.com", Age = 30 };
            string insertSql = "INSERT INTO Users (Name, Email, Age) VALUES (@Name, @Email, @Age)";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int rowsAffected = db.Execute(insertSql, newUser);
                Console.WriteLine($"Вставлено строк: {rowsAffected}");
            }

            // 2. 
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                // Получаем всех пользователей старше 25 лет
                var users = db.Query<User>("SELECT * FROM Users WHERE Age > @Age", new { Age = 25 }).ToList();
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}, Имя: {user.Name}, Email: {user.Email}, Возраст: {user.Age}");
                }
            }

            // 3. 
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var updateSql = "UPDATE Users SET Age = @Age WHERE Name = @Name";
                db.Execute(updateSql, new { Age = 31, Name = "Иван Петров" });
                Console.WriteLine("Возраст Ивана успешно обновлен на 31.");
            }

            // 4. 
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var deleteSql = "DELETE FROM Users WHERE Name = @Name";
                db.Execute(deleteSql, new { Name = "Иван Петров" });
                Console.WriteLine("Запись Ивана удалена из базы.");
            }

            Console.WriteLine("\nГотово! Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}
