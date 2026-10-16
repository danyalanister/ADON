using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public int Age { get; set; }
    public double GPA { get; set; }
    public int PassedCredits { get; set; }

    public override string ToString()
    {
        return $"{Name,-10} | {Group,-6} | {Age,2} | {GPA,4:F1} | {PassedCredits,2}";
    }
}

class Program
{
    private static List<Student> GetStudents()
    {
        return new List<Student>
        {
            new Student { Name = "Алексей", Group = "ИТ-41", Age = 20, GPA = 4.2, PassedCredits = 40 },
            new Student { Name = "Борис", Group = "ИТ-41", Age = 19, GPA = 3.1, PassedCredits = 35 },
            new Student { Name = "Виктория", Group = "ПИ-42", Age = 21, GPA = 4.8, PassedCredits = 45 },
            new Student { Name = "Григорий", Group = "ИБ-43", Age = 20, GPA = 2.9, PassedCredits = 20 },
            new Student { Name = "Дарья", Group = "ИТ-41", Age = 22, GPA = 4.5, PassedCredits = 42 },
            new Student { Name = "Елена", Group = "ПИ-42", Age = 20, GPA = 3.8, PassedCredits = 38 },
            new Student { Name = "Жанна", Group = "ИБ-43", Age = 23, GPA = 4.9, PassedCredits = 48 },
            new Student { Name = "Игорь", Group = "ИТ-41", Age = 19, GPA = 2.5, PassedCredits = 15 }
        };
    }

    static void Main(string[] args)
    {
        // Вызываем все задания по очереди
        Task1_QuerySyntax();
        Task2_MethodSyntax();
        Task3_Aggregation();
        Task4_Xml();

        Console.WriteLine("Нажмите Enter для выхода...");
        Console.ReadLine();
    }

    static void Task1_QuerySyntax()
    {
        var students = GetStudents();

        // Задание 1: Используем синтаксис запросов
        var goodStudents = from s in students
                           where s.Group == "ИТ-41" && s.GPA > 3.5
                           orderby s.Age
                           select s;

        Console.WriteLine("Задание 1: Студенты ИТ-41 с баллом > 3.5");
        Console.WriteLine("Имя        | Группа | Возраст | Балл | Зачеты");
        foreach (var s in goodStudents)
        {
            Console.WriteLine(s.ToString());
        }
        Console.WriteLine();
    }

    static void Task2_MethodSyntax()
    {
        var students = GetStudents();

        // Задание 2: Используем синтаксис методов и анонимные типы
        var scholarshipStudents = students
            .OrderByDescending(s => s.GPA)
            .Select(s => new
            {
                FullName = s.Name,
                Scholarship = s.GPA >= 4.7 ? "Ректорская" :
                              s.GPA >= 4.5 ? "Повышенная" :
                              s.GPA >= 4.0 ? "Стандартная" : "Нет"
            })
            .Where(x => x.Scholarship != "Нет"); 

        Console.WriteLine("-- Задание 2: Стипендии (синтаксис методов) --");
        foreach (var item in scholarshipStudents)
        {
            Console.WriteLine($"{item.FullName}: {item.Scholarship}");
        }
        Console.WriteLine();
    }

    static void Task3_Aggregation()
    {
        var students = GetStudents();

        Console.WriteLine("-- Задание 3: Статистика --");

        // Часть А: Группировка по группам и подсчет среднего балла
        var groups = students
            .GroupBy(s => s.Group)
            .OrderBy(g => g.Key);

        foreach (var g in groups)
        {
            double avg = g.Average(s => s.GPA);
            Console.WriteLine($"Группа {g.Key}: Средний балл {avg:F2}");
        }

        // Часть Б: Поиск самого старшего студента с 45 зачетами
        var oldest45 = students
            .Where(s => s.PassedCredits == 45)
            .OrderByDescending(s => s.Age)
            .FirstOrDefault(); 

        if (oldest45 != null)
        {
            Console.WriteLine($"Студент с 45 зачетами: {oldest45.Name} ({oldest45.Age} год)");
        }
        Console.WriteLine();
    }

    static void Task4_Xml()
    {
        // Часть А: Создание XML файла 
        XDocument xmlDoc = new XDocument(
            new XElement("university",
                new XElement("course", new XAttribute("name", "Программирование"),
                    new XElement("student", new XAttribute("name", "Кирилл"), new XAttribute("gpa", "3.9")),
                    new XElement("student", new XAttribute("name", "Лариса"), new XAttribute("gpa", "4.7")),
                    new XElement("student", new XAttribute("name", "Михаил"), new XAttribute("gpa", "3.2")),
                    new XElement("student", new XAttribute("name", "Наталья"), new XAttribute("gpa", "4.1"))
                )
            )
        );
        xmlDoc.Save("uni.xml"); 

        // Часть Б: Чтение и фильтрация отличников
        XDocument loadedDoc = XDocument.Load("uni.xml");

        var goodStudents = loadedDoc.Descendants("student")
            .Where(x => (double)x.Attribute("gpa") > 4.0); 
        Console.WriteLine("-- Задание 4: Отличники из XML --");
        foreach (var s in goodStudents)
        {
            Console.WriteLine(s.Attribute("name").Value);
        }
        Console.WriteLine();
    }
}