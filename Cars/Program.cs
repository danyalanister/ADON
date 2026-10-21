using System;
using System.Linq;

namespace CarRental
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА: АВТОПАРК\n");

            Task1_AddInitialData();
            Task2_GetAllCars();
            Task3_FindCarByModel("Camry");
            Task4_GetCarById(1);
            Task5_GetCarsWithBrandInfo();
            Task6_AddNewCar("X6", 2023, 150.00m, 2);
            Task7_UpdateCarPrice(1, 200.00m);

            Console.WriteLine("\nГотово! Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        static void Task1_AddInitialData()
        {
            Console.WriteLine("Добавление начальных данных.");

            using (var db = new CarRentalEntities())
            {
                if (!db.Brands.Any())
                {
                    var toyota = new Brands { Name = "Toyota", Country = "Япония" };
                    var bmw = new Brands { Name = "BMW", Country = "Германия" };

                    db.Brands.Add(toyota);
                    db.Brands.Add(bmw);
                    db.SaveChanges();

                    db.Cars.Add(new Cars { Model = "Camry", ReleaseYear = 2020, PricePerDay = 50.00m, BrandId = toyota.Id });
                    db.Cars.Add(new Cars { Model = "Corolla", ReleaseYear = 2019, PricePerDay = 40.00m, BrandId = toyota.Id });
                    db.Cars.Add(new Cars { Model = "X5", ReleaseYear = 2022, PricePerDay = 100.00m, BrandId = bmw.Id });

                    db.SaveChanges();
                    Console.WriteLine("Данные успешно добавлены!");
                }
                else
                {
                    Console.WriteLine("База данных уже содержит записи.");
                }
            }
        }

        static void Task2_GetAllCars()
        {
            Console.WriteLine("\nЗАДАНИЕ 1: Все автомобили по алфавиту");
            using (var db = new CarRentalEntities())
            {
                var cars = db.Cars.OrderBy(c => c.Model).ToList();
                foreach (var car in cars)
                {
                    Console.WriteLine($"Модель: {car.Model}, Цена за день: {car.PricePerDay}$");
                }
            }
        }

        static void Task3_FindCarByModel(string modelName)
        {
            Console.WriteLine($"\nЗАДАНИЕ 2: Поиск машины '{modelName}'");
            using (var db = new CarRentalEntities())
            {
                var car = db.Cars.FirstOrDefault(c => c.Model == modelName);
                if (car != null)
                {
                    Console.WriteLine($"Найдена машина: {car.Model}, Год выпуска: {car.ReleaseYear}");
                }
                else
                {
                    Console.WriteLine("Машина не найдена");
                }
            }
        }

        static void Task4_GetCarById(int id)
        {
            Console.WriteLine($"\nЗАДАНИЕ 3: Поиск по ID ({id})");
            using (var db = new CarRentalEntities())
            {
                var car = db.Cars.Find(id);
                if (car != null)
                {
                    Console.WriteLine($"По ID {id} найдена модель: {car.Model}");
                }
                else
                {
                    Console.WriteLine("Машина с таким ID не найдена");
                }
            }
        }

        static void Task5_GetCarsWithBrandInfo()
        {
            Console.WriteLine("\nЗАДАНИЕ 4: Автомобили с названием марки");
            using (var db = new CarRentalEntities())
            {
                var cars = db.Cars.ToList();
                foreach (var car in cars)
                {
                    Console.WriteLine($"Модель: {car.Model,-10} | Марка: {car.Brands.Name,-8} | Страна: {car.Brands.Country}");
                }
            }
        }

        static void Task6_AddNewCar(string model, int year, decimal price, int brandId)
        {
            Console.WriteLine($"\nЗАДАНИЕ 5: Добавление машины '{model}'");
            using (var db = new CarRentalEntities())
            {
                var newCar = new Cars { Model = model, ReleaseYear = year, PricePerDay = price, BrandId = brandId };
                db.Cars.Add(newCar);
                db.SaveChanges();
                Console.WriteLine($"Машина {model} успешно добавлена в базу!");
            }
        }

        static void Task7_UpdateCarPrice(int carId, decimal newPrice)
        {
            Console.WriteLine($"\nЗАДАНИЕ 6: Обновление цены машины (ID: {carId})");
            using (var db = new CarRentalEntities())
            {
                var car = db.Cars.Find(carId);
                if (car != null)
                {
                    car.PricePerDay = newPrice;
                    db.SaveChanges();
                    Console.WriteLine($"Цена для машины {car.Model} успешно обновлена на {newPrice}$!");
                }
                else
                {
                    Console.WriteLine("Машина для обновления не найдена");
                }
            }
        }
    }
}