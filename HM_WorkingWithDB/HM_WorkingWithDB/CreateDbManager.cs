using Npgsql;

namespace HM_WorkingWithDB
{
    internal static class CreateDbManager
    {
        public static void Init(NpgsqlConnection connection)
        {
            var dbName = "advertisement_app";
            if (!CheckDatabaseExists(connection, dbName))
            {
                CreateDatabase(connection, dbName);
            }

            SetUsingDatabase(connection, dbName);
            
            if (!CheckTableExists(connection, "users", dbName))
            {
                CreateUsersTable(connection);
            }

            if (!CheckTableExists(connection, "categories", dbName))
            {
                CreateCategoriesTable(connection);
            }

            if (!CheckTableExists(connection, "advertisements", dbName))
            {
                CreateAdvertisementsTable(connection);
            }

            Console.WriteLine("Database Init successfully");
        }

        static bool CheckDatabaseExists(NpgsqlConnection connection, string dbName)
        {
            var sql = @"
                SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname=:database_name);
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            var parameters = cmd.Parameters;
            parameters.Add(new NpgsqlParameter("database_name", dbName));
            var result = cmd.ExecuteScalar();

            return Convert.ToBoolean(result);

        }

        static void SetUsingDatabase(NpgsqlConnection connection, string dbName)
        {
            var sql ="SET search_path = " + dbName + ";";
            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
        static void CreateDatabase(NpgsqlConnection connection, string dbName)
        {
            var sql = "CREATE DATABASE " + dbName + @"
                OWNER = postgres
                ENCODING UTF8;
            ";

            sql += "CREATE SCHEMA " + dbName + ";";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        static void CreateUsersTable(NpgsqlConnection connection)
        {
            var sql = @"
                CREATE SEQUENCE users_id_seq;

                CREATE TABLE users
                (
                    id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('users_id_seq'),
                    first_name      CHARACTER VARYING(255)      NOT NULL,
                    last_name       CHARACTER VARYING(255)      NOT NULL,
                    middle_name     CHARACTER VARYING(255),
                    email           CHARACTER VARYING(255)      NOT NULL,
                    telephone       VARCHAR(10),
  
                    CONSTRAINT users_pkey PRIMARY KEY (id),
                    CONSTRAINT users_email_unique UNIQUE (email),
                    CONSTRAINT users_telephone_unique UNIQUE (telephone)
                );

                CREATE INDEX users_last_name_idx ON users(last_name);
                CREATE UNIQUE INDEX users_email_unq_idx ON users(lower(email));
                CREATE UNIQUE INDEX users_telephone_unq_idx ON users(telephone);
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Created USERS table");
        }

        static void CreateCategoriesTable(NpgsqlConnection connection)
        {
            var sql = @"
                CREATE SEQUENCE categories_id_seq;

                CREATE TABLE categories
                (
                    id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('categories_id_seq'),
                    name            CHARACTER VARYING(255)      NOT NULL,
  
                    CONSTRAINT categories_pkey PRIMARY KEY (id),
                    CONSTRAINT categories_name_unique UNIQUE (name)
                );

                CREATE UNIQUE INDEX categories_name_unq_idx ON categories(lower(name));
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"Created CATEGORIES table");
        }

        static void CreateAdvertisementsTable(NpgsqlConnection connection)
        {
            var sql = @"
                CREATE SEQUENCE advertisements_id_seq;

                CREATE TABLE advertisements
                (
                    id              BIGINT                      NOT NULL    DEFAULT NEXTVAL('advertisements_id_seq'),
                    user_id         BIGINT                      NOT NULL,
                    category_id     BIGINT                      NOT NULL,
                    created_at      TIMESTAMP WITH TIME ZONE    NOT NULL, 
                    name            CHARACTER VARYING(255)      NOT NULL,
                    description     CHARACTER VARYING(1024),
  
                    CONSTRAINT advertisements_pkey PRIMARY KEY (id),
                    CONSTRAINT advertisements_fk_user_id FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
                    CONSTRAINT advertisements_fk_category_id FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
                );
            ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"Created ADVERTISEMENTS table.");
        }

        static bool CheckTableExists(NpgsqlConnection connection, string tableName, string dbName)
        {
            var sql = @"SELECT EXISTS (
                            SELECT FROM 
                                pg_tables
                            WHERE 
                                schemaname = :database_name AND 
                                tablename  = :table_name
            );";

            using var cmd = new NpgsqlCommand(sql, connection);
            var parameters = cmd.Parameters;
            parameters.Add(new NpgsqlParameter("table_name", tableName));
            parameters.Add(new NpgsqlParameter("database_name", dbName));
            var result = cmd.ExecuteScalar();

            return Convert.ToBoolean(result);
        }
    }
}
