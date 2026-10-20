<Query Kind="Statements">
  <Connection>
    <ID>39cfea5b-e10a-41c2-aafa-8e271c0945ca</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>CountriesDB</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

// ЗАДАНИЕ 2
// Отобразить всю информацию о странах
Countries.Dump("Вся информация о странах");

// Отобразить название стран
Countries.Select(c => c.Name).Dump("Названия стран");

// Отобразить название столиц
Cities.Where(c => c.IsCapital == true).Select(c => c.Name).Dump("Названия столиц");

// Отобразить название крупных городов конкретной страны (например, России)
string targetCountry = "Россия";
Cities.Where(c => c.Country.Name == targetCountry && c.IsCapital == false)
      .Select(c => c.Name).Dump($"Крупные города страны: {targetCountry}");

// Отобразить название столиц с количеством жителей больше 5 миллионов
Cities.Where(c => c.IsCapital == true && c.Population > 5000000)
      .Select(c => c.Name).Dump("Столицы с населением > 5 млн");

// Отобразить название всех европейских стран
Countries.Where(c => c.Region.Name == "Европа")
         .Select(c => c.Name).Dump("Европейские страны");

// Отобразить название стран с площадью большей конкретного числа
double minArea = 1000000;
Countries.Where(c => c.Area > minArea)
         .Select(c => c.Name).Dump($"Страны с площадью больше {minArea}");


// ЗАДАНИЕ 3
// Отобразить все столицы, у которых в названии есть буквы а, р
Cities.Where(c => c.IsCapital == true && c.Name.Contains("а") && c.Name.Contains("р"))
      .Select(c => c.Name).Dump("Столицы с буквами 'а' и 'р' (например, Париж)");

// Отобразить все столицы, у которых название начинается с буквы k (в нашей БД - 'К')
Cities.Where(c => c.IsCapital == true && c.Name.StartsWith("К"))
      .Select(c => c.Name).Dump("Столицы на букву 'К' (Копенгаген)");

// Отобразить название стран, у которых площадь находится в указанном диапазоне
Countries.Where(c => c.Area >= 500000 && c.Area <= 20000000)
         .Select(c => c.Name).Dump("Страны с площадью от 0.5 до 20 млн");

// Отобразить название стран, у которых количество жителей больше указанного числа
long minPop = 100000000;
Countries.Where(c => c.Population > minPop)
         .Select(c => c.Name).Dump("Страны с населением > 100 млн");


// ЗАДАНИЕ 4
// Показать топ-5 стран по площади
Countries.OrderByDescending(c => c.Area).Take(5)
         .Select(c => c.Name).Dump("Топ-5 стран по площади");

// Показать топ-5 столиц по количеству жителей
Cities.Where(c => c.IsCapital == true).OrderByDescending(c => c.Population).Take(5)
      .Select(c => c.Name).Dump("Топ-5 столиц по населению");

// Показать страну с самой большой площадью
Countries.OrderByDescending(c => c.Area).FirstOrDefault()
         .Dump("Страна с самой большой площадью");

// Показать столицу с самым большим количеством жителей
Cities.Where(c => c.IsCapital == true).OrderByDescending(c => c.Population).FirstOrDefault()
      .Dump("Столица с самым большим населением");

// Показать страну с самой маленькой площадью в Европе
Countries.Where(c => c.Region.Name == "Европа").OrderBy(c => c.Area).FirstOrDefault()
         .Dump("Самая маленькая страна в Европе");

// Показать среднюю площадь стран в Европе
Countries.Where(c => c.Region.Name == "Европа").Average(c => c.Area)
         .Dump("Средняя площадь в Европе");

// Показать топ-3 городов по количеству жителей для конкретной страны (России)
Cities.Where(c => c.Country.Name == "Россия" && c.IsCapital == false)
      .OrderByDescending(c => c.Population).Take(3)
      .Select(c => new { Город = c.Name, Население = c.Population })
      .Dump("Топ-3 крупных городов России");

// Показать общее количество стран
Countries.Count().Dump("Общее количество стран");

// Показать часть света с максимальным количеством стран
Countries.GroupBy(c => c.Region.Name)
         .OrderByDescending(g => g.Count())
         .Select(g => g.Key).FirstOrDefault()
         .Dump("Часть света с макс. количеством стран");

// Показать количество стран в каждой части света
Countries.GroupBy(c => c.Region.Name)
         .Select(g => new { ЧастьСвета = g.Key, КоличествоСтраны = g.Count() })
         .Dump("Количество стран по частям света");