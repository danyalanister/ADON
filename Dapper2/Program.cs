using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperMailingApp
{
    // Классы-модели для маппинга данных из БД
    public class Customer
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class Promotion
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    class Program
    {
        static string connectionString = "Server=localhost;Database=PromoMailingDb;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //ЗАДАНИЕ 2
            Console.WriteLine("ЗАДАНИЕ 2: Проверка подключения");
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    Console.WriteLine("[УСПЕХ] Успешное подключение к базе данных «Список рассылки»!");
                    db.Close();
                    Console.WriteLine("[УСПЕХ] Отключение от базы данных выполнено корректно.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ОШИБКА] Не удалось подключиться к БД: {ex.Message}");
                    return;
                }
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //ЗАДАНИЕ 3
                Console.WriteLine("ЗАДАНИЕ 3: Общие данные");

                var customers = db.Query<Customer>("SELECT FullName, Email, Country, City FROM Customers").ToList();
                Console.WriteLine("\n1. Все покупатели:");
                customers.ForEach(c => Console.WriteLine($"   - {c.FullName} ({c.Country}, {c.City})"));

                Console.WriteLine("\n2. Email всех покупателей:");
                customers.ForEach(c => Console.WriteLine($"   - {c.Email}"));

                var categories = db.Query<string>("SELECT Name FROM Categories").ToList();
                Console.WriteLine("\n3. Список разделов (категорий):");
                categories.ForEach(c => Console.WriteLine($"   - {c}"));

                var promos = db.Query<Promotion>("SELECT Name, Country, StartDate, EndDate FROM Promotions").ToList();
                Console.WriteLine("\n4. Список акционных товаров:");
                promos.ForEach(p => Console.WriteLine($"   - {p.Name} (Действует в {p.Country} до {p.EndDate:dd.MM.yyyy})"));

                var cities = db.Query<string>("SELECT DISTINCT City FROM Customers").ToList();
                Console.WriteLine("\n5. Все города:");
                cities.ForEach(c => Console.WriteLine($"   - {c}"));

                var countries = db.Query<string>("SELECT DISTINCT Country FROM Customers").ToList();
                Console.WriteLine("\n6. Все страны:");
                countries.ForEach(c => Console.WriteLine($"   - {c}"));


                //ЗАДАНИЕ 4
                Console.WriteLine("\nЗАДАНИЕ 4: Фильтрация по параметрам");

                string targetCity = "Чита";
                var customersInCity = db.Query<Customer>("SELECT * FROM Customers WHERE City = @City", new { City = targetCity }).ToList();
                Console.WriteLine($"\n1. Покупатели из города {targetCity}:");
                customersInCity.ForEach(c => Console.WriteLine($"   - {c.FullName}"));

                string targetCountry = "Россия";
                var customersInCountry = db.Query<Customer>("SELECT * FROM Customers WHERE Country = @Country", new { Country = targetCountry }).ToList();
                Console.WriteLine($"\n2. Покупатели из страны {targetCountry}:");
                customersInCountry.ForEach(c => Console.WriteLine($"   - {c.FullName}"));

                var promosInCountry = db.Query<Promotion>("SELECT * FROM Promotions WHERE Country = @Country", new { Country = targetCountry }).ToList();
                Console.WriteLine($"\n3. Акции для страны {targetCountry}:");
                promosInCountry.ForEach(p => Console.WriteLine($"   - {p.Name}"));
            }

            Console.WriteLine("\nГотово! Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}
