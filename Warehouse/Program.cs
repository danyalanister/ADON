using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WarehouseApp
{
    class Program
    {
        static string connString = ConfigurationManager.ConnectionStrings["WarehouseConn"].ConnectionString;

        static void Main(string[] args)
        {
            // ЗАДАНИЕ 2: Подключение с выводом сообщения об успехе или ошибке
            if (!TestConnection())
            {
                Console.ReadLine();
                return; 
            }

            while (true)
            {
                Console.WriteLine("\nМЕНЮ СКЛАДА");
                Console.WriteLine("1. Показать всю информацию о товарах (Задание 3)");
                Console.WriteLine("2. Показать все типы товаров (Задание 3)");
                Console.WriteLine("3. Показать всех поставщиков (Задание 3)");
                Console.WriteLine("4. Товар с макс/мин количеством и себестоимостью (Задание 3)");
                Console.WriteLine("5. Показать товары заданной категории (Задание 4)");
                Console.WriteLine("6. Показать товары заданного поставщика (Задание 4)");
                Console.WriteLine("7. Самый старый товар и среднее количество (Задание 4)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ExecuteQuery("SELECT p.ProductName, t.TypeName, s.SupplierName, p.Quantity, p.CostPrice, p.SupplyDate FROM Products p JOIN ProductTypes t ON p.TypeId = t.Id JOIN Suppliers s ON p.SupplierId = s.Id"); break;
                    case "2": ExecuteQuery("SELECT * FROM ProductTypes"); break;
                    case "3": ExecuteQuery("SELECT * FROM Suppliers"); break;
                    case "4":
                        ExecuteQuery("SELECT TOP 1 ProductName AS 'Макс количество', Quantity FROM Products ORDER BY Quantity DESC");
                        ExecuteQuery("SELECT TOP 1 ProductName AS 'Мин количество', Quantity FROM Products ORDER BY Quantity ASC");
                        ExecuteQuery("SELECT TOP 1 ProductName AS 'Мин себестоимость', CostPrice FROM Products ORDER BY CostPrice ASC");
                        ExecuteQuery("SELECT TOP 1 ProductName AS 'Макс себестоимость', CostPrice FROM Products ORDER BY CostPrice DESC");
                        break;
                    case "5":
                        Console.Write("Введите название категории (например, Электроника): ");
                        string cat = Console.ReadLine();
                        ExecuteQueryWithParam("SELECT p.ProductName, p.Quantity FROM Products p JOIN ProductTypes t ON p.TypeId = t.Id WHERE t.TypeName = @param", cat);
                        break;
                    case "6":
                        Console.Write("Введите название поставщика (например, ООО ТехПром): ");
                        string sup = Console.ReadLine();
                        ExecuteQueryWithParam("SELECT p.ProductName, p.Quantity FROM Products p JOIN Suppliers s ON p.SupplierId = s.Id WHERE s.SupplierName = @param", sup);
                        break;
                    case "7":
                        ExecuteQuery("SELECT TOP 1 ProductName AS 'Самый старый товар', SupplyDate FROM Products ORDER BY SupplyDate ASC");
                        ExecuteQuery("SELECT t.TypeName AS 'Тип', AVG(p.Quantity) AS 'Среднее кол-во' FROM Products p JOIN ProductTypes t ON p.TypeId = t.Id GROUP BY t.TypeName");
                        break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
            }
        }

        // Метод для Задания 2: Проверка подключения
        static bool TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Успешное подключение к базе данных «Склад»!");
                    Console.ResetColor();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка подключения: " + ex.Message);
                    Console.ResetColor();
                    return false;
                }
            }
        }

        // Универсальный метод для вывода таблиц на консоль 
        static void ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    PrintReader(reader);
                }
                catch (Exception ex) { Console.WriteLine("Ошибка запроса: " + ex.Message); }
            }
        }

        // Метод для безопасной передачи параметров 
        static void ExecuteQueryWithParam(string query, string paramValue)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@param", SqlDbType.NVarChar).Value = paramValue;
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    PrintReader(reader);
                }
                catch (Exception ex) { Console.WriteLine("Ошибка запроса: " + ex.Message); }
            }
        }

        // Отрисовка результатов
        static void PrintReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                Console.WriteLine("Данные не найдены.");
                return;
            }

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t| ");
            }
            Console.WriteLine("\n-");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string val = reader[i].ToString();
                    if (val.Length > 15) val = val.Substring(0, 12) + "...";
                    Console.Write(val + "\t| ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}