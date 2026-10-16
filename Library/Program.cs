using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AdoNetLab2
{
    class Program
    {
        static string cs = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Task1_ManualDataTable();
            Task2_FillDataSet();
            Task3_AutoUpdate();
            Task4_CustomUpdate();
            Task5_RowStateOrder();
            Task6_TableMappings();

            Console.WriteLine("\nГотово. Нажмите любую клавишу.");
            Console.ReadKey();
        }

        // Вспомогательный метод из методички
        static void PrintTable(DataTable table, string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                Console.WriteLine($"\n={title}=");
            }

            foreach (DataColumn col in table.Columns)
                Console.Write(col.ColumnName.PadRight(15));
            Console.WriteLine();

            Console.WriteLine(new string('-', table.Columns.Count * 15));

            foreach (DataRow row in table.Rows)
            {
                // Если строка удалена, мы не можем прочитать ее данные напрямую, будет ошибка
                if (row.RowState == DataRowState.Deleted)
                {
                    Console.WriteLine("Строка удалена".PadRight(15));
                    continue;
                }

                foreach (DataColumn col in table.Columns)
                {
                    object val = row[col];
                    string s = val == DBNull.Value ? "NULL" : val.ToString();
                    Console.Write(s.PadRight(15));
                }
                Console.WriteLine();
            }
        }

        // Задание 1.
        static void Task1_ManualDataTable()
        {
            DataTable table = new DataTable("Authors");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));

            table.Rows.Add(1, "Isaac", "Asimov");
            table.Rows.Add(2, "Ray", "Bradbury");
            table.Rows.Add(3, "Clifford", "Simak");

            DataRow newRow = table.NewRow();
            newRow[0] = 4;
            newRow["FirstName"] = "Roger";
            newRow["LastName"] = "Zelazny";
            table.Rows.Add(newRow);

            PrintTable(table, "Authors (manual)");
        }

        // Задание 2.
        static void Task2_FillDataSet()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Authors", conn);
                DataSet ds = new DataSet();

                // Метод Fill сам открывает и закрывает подключение
                da.Fill(ds, "Authors");

                PrintTable(ds.Tables["Authors"], "Авторы из БД (Task 2)");
            }
        }

        // Задание 3. 
        static void Task3_AutoUpdate()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();

                da.Fill(ds, "Books");

                Console.WriteLine("\n--- Команды от SqlCommandBuilder ---");
                Console.WriteLine("INSERT: " + cmdBuilder.GetInsertCommand().CommandText);
                Console.WriteLine("UPDATE: " + cmdBuilder.GetUpdateCommand().CommandText);
                Console.WriteLine("DELETE: " + cmdBuilder.GetDeleteCommand().CommandText);

                PrintTable(ds.Tables["Books"], "Книги ДО изменения (Task 3)");

                // Меняем цену первой книги
                if (ds.Tables["Books"].Rows.Count > 0)
                {
                    ds.Tables["Books"].Rows[0]["Price"] = 999;
                }

                PrintTable(ds.Tables["Books"], "Книги ПОСЛЕ локального изменения (Task 3)");

                // Синхронизируем с БД
                da.Update(ds, "Books");

                // Перечитываем, чтобы убедиться, что всё сохранилось
                DataSet dsCheck = new DataSet();
                da.Fill(dsCheck, "Books");
                PrintTable(dsCheck.Tables["Books"], "Книги после перечитывания из БД (Task 3)");
            }
        }

        // Задание 4. 
        static void Task4_CustomUpdate()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);

                SqlCommand updateCmd = new SqlCommand("UPDATE Books SET Price = @pPrice WHERE Id = @pId", conn);

                updateCmd.Parameters.Add(new SqlParameter("@pPrice", SqlDbType.Int));
                updateCmd.Parameters["@pPrice"].SourceVersion = DataRowVersion.Current;
                updateCmd.Parameters["@pPrice"].SourceColumn = "Price";

                updateCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
                updateCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
                updateCmd.Parameters["@pId"].SourceColumn = "Id";

                da.UpdateCommand = updateCmd;

                DataSet ds = new DataSet();
                da.Fill(ds, "Books");

                // Меняем цены у двух книг
                if (ds.Tables["Books"].Rows.Count >= 2)
                {
                    ds.Tables["Books"].Rows[0]["Price"] = 111;
                    ds.Tables["Books"].Rows[1]["Price"] = 222;
                }

                da.Update(ds, "Books");

                DataSet dsCheck = new DataSet();
                da.Fill(dsCheck, "Books");
                PrintTable(dsCheck.Tables["Books"], "Результат пользовательского Update (Task 4)");
            }
        }

        // Задание 5.
        static void Task5_RowStateOrder()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataTable books = new DataTable();
                da.Fill(books);

                if (books.Rows.Count >= 2)
                {
                    books.Rows[0].Delete();
                    books.Rows[1]["Price"] = 777;
                }

                DataRow newRow = books.NewRow();
                newRow["AuthorId"] = 1;
                newRow["Title"] = "New Book";
                newRow["Price"] = 500;
                newRow["Pages"] = 100;
                books.Rows.Add(newRow);

                Console.WriteLine("\nСостояния строк (Task 5)");
                foreach (DataRow row in books.Rows)
                {
                    // Проверка на удаление, так как у удаленной строки нельзя прочитать Title просто так
                    string title = row.RowState == DataRowState.Deleted ? "Удалено" : row["Title"].ToString();
                    Console.WriteLine($"{title,-20} -> {row.RowState}");
                }

                // Выполняем в правильном порядке
                da.Update(books.Select(null, null, DataViewRowState.Deleted));
                da.Update(books.Select(null, null, DataViewRowState.ModifiedCurrent));
                da.Update(books.Select(null, null, DataViewRowState.Added));

                DataTable checkTable = new DataTable();
                da.Fill(checkTable);
                PrintTable(checkTable, "Таблица после правильной синхронизации (Task 5)");
            }
        }

        // Задание 6.
        static void Task6_TableMappings()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string sql = "SELECT * FROM Authors; SELECT * FROM Books;";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                // Настраиваем переименование
                da.TableMappings.Add("Table", "Authors");
                da.TableMappings.Add("Table1", "Books");

                // Переименовываем колонки для Authors
                var dtm = da.TableMappings["Table"];
                dtm.ColumnMappings.Add("Id", "Num");
                dtm.ColumnMappings.Add("FirstName", "Name");
                dtm.ColumnMappings.Add("LastName", "Surname");

                DataSet ds = new DataSet();
                da.Fill(ds);

                // Выводим таблицы, используя новые красивые имена
                PrintTable(ds.Tables["Authors"], "Authors mapped (Task 6)");
                PrintTable(ds.Tables["Books"], "Books mapped (Task 6)");
            }
        }
    }
}