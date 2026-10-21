<Query Kind="Statements">
  <Connection>
    <ID>d49b82be-f775-46a7-b0ef-8587515a1d08</ID>
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

// ЗАДАНИЕ 1: Добавление данных (Insert)
"ЗАДАНИЕ 1: ДОБАВЛЕНИЕ".Dump();

var region = Regions.FirstOrDefault(r => r.Name == "Европа");

// Добавляем Италию
var newCountry = new Countries 
{ 
    Name = "Италия", 
    RegionId = region.Id, 
    Area = 301340, 
    Population = 59000000 
};
Countries.InsertOnSubmit(newCountry);
SubmitChanges(); 

// Добавляем Рим и Милан
var newCapital = new Cities 
{ 
    Name = "Рим", 
    CountryId = newCountry.Id, 
    Population = 2800000, 
    IsCapital = true 
};
var newCity = new Cities 
{ 
    Name = "Милан", 
    CountryId = newCountry.Id, 
    Population = 1300000, 
    IsCapital = false 
};

Cities.InsertOnSubmit(newCapital);
Cities.InsertOnSubmit(newCity);
SubmitChanges();

"Италия, Рим и Милан успешно добавлены в базу данных!".Dump();


// ЗАДАНИЕ 2: Обновление данных (Update)
"ЗАДАНИЕ 2: ОБНОВЛЕНИЕ".Dump();

var countryToUpdate = Countries.FirstOrDefault(c => c.Name == "Италия");
if (countryToUpdate != null)
{
    countryToUpdate.Population = 60000000;
}

var capitalToUpdate = Cities.FirstOrDefault(c => c.Name == "Рим");
if (capitalToUpdate != null)
{
    capitalToUpdate.Population = 3000000; 
}

SubmitChanges();
"Данные о населении Италии и Рима успешно обновлены!".Dump();


// ЗАДАНИЕ 4: Вызов хранимых процедур (Stored Procedures)
"ЗАДАНИЕ 4: ХРАНИМЫЕ ПРОЦЕДУРЫ".Dump();

this.ExecuteQuery<Countries>("EXEC sp_GetTop3Countries")
    .Dump("Топ-3 стран по количеству жителей");

this.ExecuteQuery<Cities>("EXEC sp_GetTop3Capitals")
    .Dump("Топ-3 столиц по количеству жителей");

this.ExecuteQuery<Countries>("EXEC sp_GetMaxPopulationCountry")
    .Dump("Страна с самым большим количеством жителей");

this.ExecuteQuery<Cities>("EXEC sp_GetMaxPopulationCity")
    .Dump("Город с самым большим количеством жителей");


// ЗАДАНИЕ 3: Удаление данных (Delete)
"ЗАДАНИЕ 3: УДАЛЕНИЕ".Dump();

// Удаляем города, привязанные к стране
var citiesToDelete = Cities.Where(c => c.Country.Name == "Италия");
Cities.DeleteAllOnSubmit(citiesToDelete);

// Удаляем страну
var countryToDelete = Countries.FirstOrDefault(c => c.Name == "Италия");
if (countryToDelete != null)
{
    Countries.DeleteOnSubmit(countryToDelete);
}

SubmitChanges();
"Тестовые данные (Италия, Рим, Милан) успешно удалены из базы!".Dump();