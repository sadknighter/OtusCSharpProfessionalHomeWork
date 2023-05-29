using Dapper;
using HM_WorkingWithDB.Entities;
using System.Data;

namespace HM_WorkingWithDB.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private IDbConnection _dbConnection;

        public CategoryRepository(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public void AddRecord(Category record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();
            var sql = @"INSERT INTO advertisement_app.categories(name)
                            VALUES(@Name)
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
            int affectedRowsCount = _dbConnection.Execute(@"DELETE FROM advertisement_app.categories WHERE Id = @Id", new { Id = id });
            transaction.Commit();

            return affectedRowsCount;
        }

        public bool ExistRecord(Category record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            object result = _dbConnection.ExecuteScalar
                (@"select EXISTS(select 1 from advertisement_app.categories where name = @Name)",
                new
                {
                    record.Name,
                });

            return Convert.ToBoolean(result);
        }

        public IEnumerable<Category> GetAllRecords()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            IEnumerable<Category> categories = _dbConnection.Query<Category>(
                @"SELECT 
                        id Id,
                        name Name
                      FROM advertisement_app.categories c
                ORDER BY c.name");

            return categories;
        }

        public Category GetRecord(long id)
        {
            throw new NotImplementedException();
        }

        public int UpdateRecord(Category record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();

            int affectedRowsCount = _dbConnection.Execute
                (@"UPDATE advertisement_app.categories SET 
                                    name = @Name
                                    WHERE categories.id=@ToUpdateId",
                new
                {
                    ToUpdateId = record.Id,
                    record.Name
                },
                transaction);

            transaction.Commit();

            return affectedRowsCount;
        }
    }
}
