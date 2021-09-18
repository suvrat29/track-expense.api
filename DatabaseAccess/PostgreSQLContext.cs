using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.DatabaseAccess
{
    public class PostgreSQLContext : DbContext
    {
        #region Variables
        private string _connectionString = "";
        #endregion

        #region TableProperties
        public DbSet<UserModelVM> user {get; set;}
        public DbSet<ApplicationlogVM> applog { get; set; }
        public DbSet<LocalesVM> locale { get; set; }
        public DbSet<EmaildataVM> emaildata { get; set; }
        public DbSet<CategorydataVM> categorydata { get; set; }
        public DbSet<SubcategorydataVM> subcategorydata { get; set; }
        public DbSet<UseractivitylogVM> useractivitylog { get; set; }
        #endregion

        #region Constructor
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
            //Build connectionstring for heroku postgres
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // parse the connection string
            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
            /////////////////////////////////////////////////
            ///
            _connectionString = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }
        #endregion

        #region Methods

        #region Protected Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #endregion

        #region Public Methods
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// This function allows easy mapping of queryResult and viewmodels. Can be used in any function to map a non standard data model to a query result
        /// </summary>
        public T getDataFromQuery<T>(string query) where T : new()
        {
            T queryResult = new T();

            // Connect to the database
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                // Define a query
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Execute the query and obtain a result set
                    NpgsqlDataReader dr = command.ExecuteReader();

                    // Output rows
                    try
                    {
                        while (dr.Read())
                        {
                            T obj = new T();

                            foreach (var prop in obj.GetType().GetProperties())
                            {
                                try
                                {
                                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                                    propertyInfo.SetValue(obj, Convert.ChangeType(dr[prop.Name], propertyInfo.PropertyType), null);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            queryResult = obj;
                        }
                    }
                    catch
                    {
                        return default(T);
                    }

                    conn.Close();

                    return queryResult;
                }
            }
        }

        /// <summary>
        /// This function allows easy mapping of queryResult and viewmodels. Can be used in any function to map a non standard data model to a query result
        /// </summary>
        public List<T> getDataFromQueryAsList<T>(string query) where T : new()
        {
            List<T> queryResult = new List<T>();

            // Connect to the database
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                // Define a query
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Execute the query and obtain a result set
                    NpgsqlDataReader dr = command.ExecuteReader();

                    // Output rows
                    try
                    {
                        while (dr.Read())
                        {
                            T obj = new T();

                            foreach (var prop in obj.GetType().GetProperties())
                            {
                                try
                                {
                                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                                    propertyInfo.SetValue(obj, Convert.ChangeType(dr[prop.Name], propertyInfo.PropertyType), null);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            queryResult.Add(obj);
                        }
                    }
                    catch
                    {
                        return default(List<T>);
                    }

                    conn.Close();

                    return queryResult;
                }
            }
        }

        /// <summary>
        /// This function allows easy mapping of queryResult and viewmodels. Can be used in any function to map a non standard data model to a query result
        /// </summary>
        public async Task<T> getDataFromQueryAsync<T>(string query) where T: new()
        {
            T queryResult = new T();

            // Connect to the database
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Define a query
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Execute the query and obtain a result set
                    NpgsqlDataReader dr = await command.ExecuteReaderAsync();

                    // Output rows
                    try
                    {
                        while (dr.Read())
                        {
                            T obj = new T();

                            foreach (var prop in obj.GetType().GetProperties())
                            {
                                try
                                {
                                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                                    propertyInfo.SetValue(obj, Convert.ChangeType(dr[prop.Name], propertyInfo.PropertyType), null);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            queryResult = obj;
                        }
                    }
                    catch
                    {
                        return default(T);
                    }

                    await conn.CloseAsync();

                    return queryResult;
                }
            }
        }

        /// <summary>
        /// This function allows easy mapping of queryResult and viewmodels. Can be used in any function to map a non standard data model to a query result
        /// </summary>
        public async Task<List<T>> getDataFromQueryAsListAsync<T>(string query) where T: new()
        {
            List<T> queryResult = new List<T>();

            // Connect to the database
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Define a query
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Execute the query and obtain a result set
                    NpgsqlDataReader dr = await command.ExecuteReaderAsync();

                    // Output rows
                    try
                    {
                        while (dr.Read())
                        {
                            T obj = new T();

                            foreach (var prop in obj.GetType().GetProperties())
                            {
                                try
                                {
                                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                                    propertyInfo.SetValue(obj, Convert.ChangeType(dr[prop.Name], propertyInfo.PropertyType), null);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            queryResult.Add(obj);
                        }
                    }
                    catch
                    {
                        return default(List<T>);
                    }

                    await conn.CloseAsync();

                    return queryResult;
                }
            }
        }
        #endregion

        #endregion
    }
}
