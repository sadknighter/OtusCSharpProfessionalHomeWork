// See https://aka.ms/new-console-template for more information
using HM_WorkingWithDB;
using HM_WorkingWithDB.Entities;
using Npgsql;

Console.WriteLine("Hello, World!");
var manager = new ConfigSettingsManager();
using var connection = new NpgsqlConnection(manager.GetConnectionString());
connection.Open();

CreateDbManager.Init(connection);

var fillDbManager = new FillDbManager(connection);

var dbUsers = fillDbManager.FillDbUsers().ToList();

var dbCategories = fillDbManager.FillDbCategories().ToList();

var dbAdvertisements = fillDbManager.FillDbAdvertisements(dbUsers, dbCategories).ToList();

var isContinue = true;
while (isContinue)
{
    Console.WriteLine("\n ****************************");
    Console.WriteLine("\nFor exit input 'exit'");
    Console.Write("Please choose a table for insert data (variants: users, categories, advertisements): ");
    var enteredVariant = Console.ReadLine();
    switch (enteredVariant)
    {
        case "users":
            Console.WriteLine("Insert Data in one row in format: [first_name, last_name, middle_name, email, telephone]");
            var dataUserRow = Console.ReadLine();
            //TODO: Check valid format;
            dataUserRow = dataUserRow.Substring(1, dataUserRow.Length - 1);
            var rowUserValues = dataUserRow.Split(',');
            var user = new User
            {
                FirstName = rowUserValues[0],
                LastName = rowUserValues[1],
                MiddleName = rowUserValues[2],
                Email = rowUserValues[3],
                Telephone = rowUserValues[4],
            };

            fillDbManager.UserRepository.AddRecord(user);
            dbUsers.Add(user);
            break;
        case "categories":
            Console.WriteLine("Insert Data in one row in format: [name]");
            var dataCategoryRow = Console.ReadLine();
            //TODO: Check valid format;
            dataCategoryRow = dataCategoryRow.Substring(1, dataCategoryRow.Length - 1);
            var rowCategoryValues = dataCategoryRow.Split(',');
            var category = new Category
            {
                Name = rowCategoryValues[0]
            };

            fillDbManager.CategoryRepository.AddRecord(category);
            dbCategories.Add(category);
            break;
        case "advertisements":
            Console.WriteLine("Insert Data in one row in format: [name, description, user_id, user_last_name, user_first_name, category_id, category_name]");
            var dataAdvRow = Console.ReadLine();
            //TODO: Check valid format;
            dataAdvRow = dataAdvRow.Substring(1, dataAdvRow.Length - 1);
            var rowAdvValues = dataAdvRow.Split(',');
            var advertisement = new Advertisement
            {
                Name = rowAdvValues[0],
                Description = rowAdvValues[1],
                UserId = Convert.ToInt32(rowAdvValues[2]),
                User = new User { LastName = rowAdvValues[3], FirstName = rowAdvValues[4] },
                CategoryId = Convert.ToInt32(rowAdvValues[5]),
                Category = new Category { Id = Convert.ToInt32(rowAdvValues[5]), Name = rowAdvValues[6] }
            };

            fillDbManager.AdvertisementRepository.AddRecord(advertisement);
            dbAdvertisements.Add(advertisement);
            break;
        case "exit":
            isContinue = false;
            break;
    }
}

Console.WriteLine("\n ****************************");
Console.WriteLine("Cycle of adding data is stopped");

Console.WriteLine("\n ***** \n");
foreach (var user in dbUsers)
{
    fillDbManager.UserRepository.DeleteRecord(user.Id);
    Console.WriteLine("Deleted: " + user.ToString());
}

Console.WriteLine("\n ****************************");
foreach (var category in dbCategories)
{
    fillDbManager.CategoryRepository.DeleteRecord(category.Id);
    Console.WriteLine("Deleted: " + category.ToString());
}

Console.WriteLine("\n ****************************");
foreach (var advertisement in dbAdvertisements)
{
    fillDbManager.AdvertisementRepository.DeleteRecord(advertisement.Id);
    Console.WriteLine("Deleted: " + advertisement.ToString());
}

connection.Close();