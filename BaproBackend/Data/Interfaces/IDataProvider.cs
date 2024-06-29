namespace BaproBackend.Data.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<T>> GetAll<T>(string tablename);
        Task<T> GetByID<T>(string tablename, string Id);
        Task<int> Insert<T>(string tableName, T entity);
        Task<int> Update<T>(string tableName, T entity);
        Task<int> Delete<T>(string tableName, string Id);
        Task<IEnumerable<T>> GetAllByCondition<T>(string tableName, T entity);
    }
}
