using BaproBackend.Data.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace BaproBackend.Data
{
    public class DataProvider:IDataProvider
    {
        private readonly IDbConnection connection;
        public DataProvider()
        {
            connection = new MySqlConnection(Constants.connectionstring);
        }

        public async Task<IEnumerable<T>> GetAll<T>(string tablename)
        {
            string query = string.Format(Constants.SelectQuery, tablename);
            return await connection.QueryAsync<T>(query);
        }

        public async Task<T> GetByID<T>(string tablename, string Id)
        {
            string query = string.Format(Constants.SelectById, tablename, Id);
            return await connection.QuerySingleAsync<T>(query, new { id = Id });
        }

        public async Task<int> Insert<T>(string tableName, T entity)
        {
            var properties = typeof(T).GetProperties();
            var columnNames = properties.Select(p => p.Name);
            var columns = string.Join(',', columnNames);
            string values = string.Join(',', columnNames.Select(p => $"@{p}"));
            string query = string.Format(Constants.InsertQuery, tableName, columns, values);
            return await connection.ExecuteAsync(query, entity);
        }

        public async Task<int> Update<T>(string tableName, T entity)
        {
            var properties = typeof(T).GetProperties();
            List<string> sets = new List<string>();
            foreach(var prop in properties)
            {
                if(prop.Name!="id"&& prop.GetValue(entity) != null)
                {
                    sets.Add($"{prop.Name}=@{prop.Name}");
                }
            }
            if( sets.Count > 0 )
            {
                return 0;
            }
            string query = string.Format(Constants.UpdateQuery, tableName, string.Join(",", sets));
            return await connection.ExecuteAsync(query, entity);
            
        }

        public async Task<int> Delete<T>(string tableName, string Id)
        {
            string query = string.Format(Constants.DeleteQuery, tableName);
            return await connection.ExecuteAsync(query, new {id = Id});
        }

        public async Task<IEnumerable<T>> GetAllByCondition<T>(string tableName, T entity)
        {
            string condition = "";
            var properties = typeof (T).GetProperties();
            foreach(var prop in properties)
            {
                var data = prop.GetValue(entity);
                if(data != null)
                {
                    condition += $"{prop.Name}=@{prop.Name} and ";
                }
            }
            condition = condition.Substring(0, condition.Length-4);
            string query = string.Format(Constants.SelectByCondition, tableName, condition);
            return await connection.QueryAsync<T>(query, entity);
        }

    }
}
