using System;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StudentGradesApp
{
    class Program
    {
        // Переменная для хранения текущей базы данных
        static string currentDb = "Db1";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("ОЦЕНКИ СТУДЕНТОВ");
                Console.WriteLine("Текущая база: " + currentDb);
                Console.WriteLine("1. Сменить базу данных (Задание 6)");
                Console.WriteLine("2. Показать все оценки (Асинхронное чтение - Задание 3)");
                Console.WriteLine("3. Обновить оценку студенту (Асинхронное обновление - Задание 4)");
                Console.WriteLine("4. Удалить запись по ID (Асинхронное удаление - Задание 5)");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1") SwitchDatabase();
                else if (choice == "2") await ReadDataAsync();
                else if (choice == "3") await UpdateDataAsync();
                else if (choice == "4") await DeleteDataAsync();
                else if (choice == "0") break;
                else Console.WriteLine("Неверный ввод.");

                Console.WriteLine("\nНажмите Enter для продолжения.");
                Console.ReadLine();
            }
        }

        // Задание 6: Смена СУБД
        static void SwitchDatabase()
        {
            if (currentDb == "Db1")
            {
                currentDb = "Db2";
            }
            else
            {
                currentDb = "Db1";
            }
            Console.WriteLine("База данных успешно изменена на " + currentDb);
        }

        // Задания 1 и 2: Получение подключения через фабрику провайдеров
        static DbConnection GetConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[currentDb];

            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);

            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return conn;
        }

        // Задание 3: Асинхронное чтение
        static async Task ReadDataAsync()
        {
            using (DbConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Grades";

                Stopwatch sw = new Stopwatch();
                sw.Start(); // Задание 7: Старт таймера

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    sw.Stop();
                    Console.WriteLine("Время выполнения запроса: " + sw.Elapsed.TotalSeconds + " секунд.\n");

                    Console.WriteLine("Id \t| Студент \t\t| Предмет \t| Оценка");
                    Console.WriteLine("-");

                    while (await reader.ReadAsync())
                    {
                        string id = reader["Id"].ToString();
                        string name = reader["StudentName"].ToString().PadRight(20);
                        string subject = reader["Subject"].ToString().PadRight(15);
                        string score = reader["Score"].ToString();

                        Console.WriteLine($"{id}\t| {name}\t| {subject}\t| {score}");
                    }
                }
            }
        }

        // Задание 4: Асинхронное обновление
        static async Task UpdateDataAsync()
        {
            Console.Write("Введите ID записи для обновления оценки: ");
            string idInput = Console.ReadLine();

            Console.Write("Введите новую оценку (например, 5): ");
            string scoreInput = Console.ReadLine();

            using (DbConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Grades SET Score = " + scoreInput + " WHERE Id = " + idInput;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                sw.Stop();
                Console.WriteLine("\nОбновлено строк: " + rowsAffected);
                Console.WriteLine("Время выполнения запроса: " + sw.Elapsed.TotalSeconds + " секунд.");
            }
        }

        // Задание 5: Асинхронное удаление
        static async Task DeleteDataAsync()
        {
            Console.Write("Введите ID записи, которую нужно удалить: ");
            string idInput = Console.ReadLine();

            using (DbConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Grades WHERE Id = " + idInput;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                sw.Stop();
                Console.WriteLine("\nУдалено строк: " + rowsAffected);
                Console.WriteLine("Время выполнения запроса: " + sw.Elapsed.TotalSeconds + " секунд.");
            }
        }
    }
}
