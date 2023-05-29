using Dapper;
using HM_WorkingWithDB.Entities;
using System.Data;

namespace HM_WorkingWithDB.Repositories
{
    public class AdvertisementRepository : IRepository<Advertisement>
    {
        private IDbConnection _dbConnection;

        public AdvertisementRepository(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        /*
                 public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
         */
        public void AddRecord(Advertisement record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();
            var sql = @"INSERT INTO advertisement_app.advertisements(user_id, category_id, created_at, name, description)
                            VALUES(@UserId, @CategoryId, @CreatedAt, @Name, @Description)
                            RETURNING id";

            var result = _dbConnection.ExecuteScalar(sql, record, transaction);
            transaction.Commit();

            record.Id = Convert.ToInt64(result);
            Console.WriteLine("Successfully added: " + record.ToString());
        }

        public int DeleteRecord(long id)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();
            int affectedRowsCount = _dbConnection.Execute(@"DELETE FROM advertisement_app.advertisements WHERE Id = @Id", new { Id = id });
            transaction.Commit();

            return affectedRowsCount;
        }

        public bool ExistRecord(Advertisement record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            object result = _dbConnection.ExecuteScalar
                (@"select EXISTS(select 1 from advertisement_app.advertisements where id = @Id)",
                new
                {
                    record.Id
                });

            return Convert.ToBoolean(result);
        }

        public IEnumerable<Advertisement> GetAllRecords()
        {
            IEnumerable<Advertisement> advertisements = _dbConnection.Query<Advertisement, User, Category, Advertisement>(
                @"SELECT 
                    d.id,
                    d.user_id UserId,
                    d.created_at CreatedAt,
                    d.id,
                    d.category_id CategoryId,
                    d.name Name,
                    d.description Description,
                    u.id,
                    u.first_name FirstName,
                    u.middle_name MiddleName,
                    u.last_name LastName,
                    u.telephone Telephone,
                    c.id,
                    c.name as Name
                FROM advertisement_app.advertisements d
                JOIN advertisement_app.users u on u.id = d.user_id
                JOIN advertisement_app.categories c on c.id = d.category_id
                ORDER BY d.Name",
                (advertisement, user, category) =>
                {
                    advertisement.User = user;
                    advertisement.Category = category;
                    return advertisement;
                },

                splitOn: "Id" // optional
            );

            return advertisements;
        }

        public Advertisement GetRecord(long id)
        {
            throw new NotImplementedException();
        }

        public int UpdateRecord(Advertisement record)
        {
            throw new NotImplementedException();
        }
    }
}
