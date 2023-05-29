using HM_WorkingWithDB.Entities;
using HM_WorkingWithDB.Repositories;
using System.Data;

namespace HM_WorkingWithDB
{
    public class FillDbManager
    {
        private IDbConnection _dbConnection;
        public readonly UserRepository UserRepository;
        public readonly CategoryRepository CategoryRepository;
        public readonly AdvertisementRepository AdvertisementRepository;
        public FillDbManager(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            UserRepository = new UserRepository(_dbConnection);
            CategoryRepository = new CategoryRepository(_dbConnection);
            AdvertisementRepository = new AdvertisementRepository(_dbConnection);
        }

        public IEnumerable<User> FillDbUsers()
        {
           
            var users = new List<User>
            {
                new User
                {
                    FirstName = "Sidor",
                    MiddleName = "Ivanovich",
                    LastName = "Petrov",
                    Email = "petrov@mail.ru",
                    Telephone = "1234567890"
                },
                new User
                {
                    FirstName = "Sidor",
                    MiddleName = "Ivanovich",
                    LastName = "Petrov1",
                    Email = "petrov1@mail.ru",
                    Telephone = "1234567891"
                },
                new User
                {
                    FirstName = "Sidor",
                    MiddleName = "Ivanovich",
                    LastName = "Petrov2",
                    Email = "petrov2@mail.ru",
                    Telephone = "1234567892"
                },
                new User
                {
                    FirstName = "Sidor",
                    MiddleName = "Ivanovich",
                    LastName = "Petrov3",
                    Email = "petrov3@mail.ru",
                    Telephone = "1234567893"
                },
                new User
                {
                    FirstName = "Sidor",
                    MiddleName = "Ivanovich",
                    LastName = "Petrov4",
                    Email = "petrov4@mail.ru",
                    Telephone = "1234567894"
                }
            };

            foreach (var user in users)
            {
                if (UserRepository.ExistRecord(user))
                {
                    UserRepository.UpdateRecord(user);
                }
                else
                {
                    UserRepository.AddRecord(user);
                }
            }

            Console.WriteLine("\n ****************************");
            Console.WriteLine("Lets watch that we have in bd:");
            var dbUsers = UserRepository.GetAllRecords().ToList();

            foreach (var user in dbUsers)
            {
                Console.WriteLine(user.ToString());
            }

            return dbUsers;
        }

        public IEnumerable<Category> FillDbCategories()
        {
            var categories = new List<Category> {
                new Category { Name = "Sport"},
                new Category { Name = "Home"},
                new Category { Name = "Transport"},
                new Category { Name = "Apartment"},
                new Category { Name = "Work"},
            };

            foreach (var category in categories)
            {
                if (CategoryRepository.ExistRecord(category))
                {
                    CategoryRepository.UpdateRecord(category);
                }
                else
                {
                    CategoryRepository.AddRecord(category);
                }
            }

            Console.WriteLine("\n ****************************");
            Console.WriteLine("Lets watch that we have in bd:");
            var dbCategories = CategoryRepository.GetAllRecords().ToList();

            foreach (var category in dbCategories)
            {
                Console.WriteLine(category.ToString());
            }

            return dbCategories;
        }

        public IEnumerable<Advertisement> FillDbAdvertisements(List<User> users, List<Category> categories)
        {
            var advertisements = new List<Advertisement> {
                new Advertisement {
                    Name = "First Adsmnt",
                    CategoryId = categories[0].Id,
                    Category = new Category { Name = categories[0].Name},
                    UserId = users[0].Id,
                    User = new User{ FirstName = users[0].FirstName, LastName = users[0].LastName},
                    CreatedAt = DateTime.Now,
                    Description = "Test description"
                },
                new Advertisement {
                    Name = "Second Adsmnt",
                    CategoryId = categories[1].Id,
                    Category = new Category { Name = categories[1].Name},
                    UserId = users[1].Id,
                    User = new User{ FirstName = users[0].FirstName, LastName = users[1].LastName},
                    CreatedAt = DateTime.Now,
                    Description = "Test description 2"
                },
                new Advertisement {
                    Name = "Third Adsmnt",
                    CategoryId = categories[2].Id,
                    Category = new Category { Name = categories[2].Name },
                    UserId = users[2].Id,
                    User = new User { FirstName = users[0].FirstName, LastName = users[2].LastName },
                    CreatedAt = DateTime.Now,
                    Description = "Test description 3"
                },
                new Advertisement {
                    Name = "Fouth Adsmnt",
                    CategoryId = categories[3].Id,
                    Category = new Category { Name = categories[3].Name },
                    UserId = users[3].Id,
                    User = new User { FirstName = users[0].FirstName, LastName = users[3].LastName },
                    CreatedAt = DateTime.Now,
                    Description = "Test description 3"
                },
                new Advertisement {
                    Name = "Fifth Adsmnt",
                    CategoryId = categories[4].Id,
                    Category = new Category { Name = categories[4].Name },
                    UserId = users[4].Id,
                    User = new User { FirstName = users[0].FirstName, LastName = users[4].LastName },
                    CreatedAt = DateTime.Now,
                    Description = "Test description 4"
                },
            };

            foreach (var advertisement in advertisements)
            {
                if (AdvertisementRepository.ExistRecord(advertisement))
                {
                    AdvertisementRepository.UpdateRecord(advertisement);
                }
                else
                {
                    AdvertisementRepository.AddRecord(advertisement);
                }
            }

            Console.WriteLine("\n ****************************");
            Console.WriteLine("Lets watch that we have in bd:");
            var dbAdvertisements = AdvertisementRepository.GetAllRecords().ToList();

            foreach (var advertisement in dbAdvertisements)
            {
                Console.WriteLine(advertisement.ToString());
            }

            return dbAdvertisements;
        }
    }
}
