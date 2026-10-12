using System;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VegsAndFruitsApp
{
    class Program
    {
        // Переменная для хранения текущей базы данных (Задание 6)
        static string currentDb = "Db1";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- ОВОЩИ И ФРУКТЫ ---");
                Console.WriteLine("Текущая СУБД/База: " + currentDb);
                Console.WriteLine("1. Сменить базу данных (Задание 6)");
                Console.WriteLine("2. Показать все продукты (Асинхронное чтение - Задание 3)");
                Console.WriteLine("3. Обновить калорийность продукта (Асинхронное обновление - Задание 4)");
                Console.WriteLine("4. Удалить продукт по ID (Асинхронное удаление - Задание 5)");
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

                Console.WriteLine("\nНажмите Enter для продолжения...");
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

        // Универсальный метод для получения настроенного подключения через фабрику (Задание 1 и 2)
        static DbConnection GetConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[currentDb];

            // Получаем фабрику провайдеров
            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);

            // Создаем подключение
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
                cmd.CommandText = "SELECT * FROM Products";

                Stopwatch sw = new Stopwatch();
                sw.Start(); // Начинаем отсчет времени

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    sw.Stop();
                    // Задание 7: Вывод времени выполнения
                    Console.WriteLine("Время выполнения запроса: " + sw.Elapsed.TotalSeconds + " секунд.\n");

                    Console.WriteLine("Id \t| Название \t| Тип \t| Цвет \t| Калории");
                    Console.WriteLine("---------------------------------------------------------");

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(reader["Id"] + "\t| " + reader["Name"] + "\t| " + reader["Type"] + "\t| " + reader["Color"] + "\t| " + reader["Calories"]);
                    }
                }
            }
        }

        // Задание 4: Асинхронное обновление
        static async Task UpdateDataAsync()
        {
            Console.Write("Введите ID продукта для обновления: ");
            string idInput = Console.ReadLine();

            Console.Write("Введите новое количество калорий: ");
            string caloriesInput = Console.ReadLine();

            using (DbConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Products SET Calories = " + caloriesInput + " WHERE Id = " + idInput;

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
            Console.Write("Введите ID продукта, который нужно удалить: ");
            string idInput = Console.ReadLine();

            using (DbConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Products WHERE Id = " + idInput;

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