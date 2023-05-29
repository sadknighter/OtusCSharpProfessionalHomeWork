using Dapper;
using HM_WorkingWithDB.Entities;
using System.Data;

namespace HM_WorkingWithDB.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private IDbConnection _dbConnection;

        public UserRepository(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public void AddRecord(User record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();
            var sql = @"INSERT INTO advertisement_app.users(first_name, last_name, middle_name, email, telephone)
                            VALUES(@FirstName, @LastName, @MiddleName, @Email, @Telephone)
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
            int affectedRowsCount = _dbConnection.Execute(@"DELETE FROM advertisement_app.users WHERE Id = @Id", new { Id = id });
            transaction.Commit();

            return affectedRowsCount;
        }

        public bool ExistRecord(User record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            object result = _dbConnection.ExecuteScalar
                (@"select EXISTS(select 1 from advertisement_app.users where last_name = @LastName 
                    and telephone= @Telephone and email= @Email)",
                new
                {
                    record.LastName,
                    record.Telephone,
                    record.Email,
                });

            return Convert.ToBoolean(result);
        }

        public IEnumerable<User> GetAllRecords()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            IEnumerable<User> users = _dbConnection.Query<User>(
                @"SELECT 
                        id Id,
                        first_name FirstName,
                        last_name LastName,
                        middle_name MiddleName,
                        email Email,
                        telephone Telephone
                      FROM advertisement_app.users u
                ORDER BY u.last_name");

            return users;
        }

        public User GetRecord(long id)
        {
            throw new NotImplementedException();
        }

        public int UpdateRecord(User record)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            var transaction = _dbConnection.BeginTransaction();

            int affectedRowsCount = _dbConnection.Execute
                (@"UPDATE advertisement_app.users SET 
                                    first_name = @FirstName,
                                    last_name = @LastName,
                                    middle_name = @MiddleName,
                                    email = @Email,
                                    telephone = @Telephone
                                    WHERE users.id=@ToUpdateId",
                new
                {
                    ToUpdateId = record.Id,
                    record.FirstName,
                    record.LastName,
                    record.MiddleName,
                    record.Email,
                    record.Telephone
                },
                transaction);

            transaction.Commit();

            return affectedRowsCount;
        }
    }
}
