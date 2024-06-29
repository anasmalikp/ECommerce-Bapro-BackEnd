namespace BaproBackend.Data
{
    public static class Constants
    {
        public const string connectionstring = "server=localhost; database=baprodb; uid=root; pwd=1234567890";
        public const string InsertQuery = "insert into {0} ({1}) values ({2})";
        public const string UpdateQuery = "update {0} set {1} where id=@id";
        public const string DeleteQuery = "delete from {0} where id=@id";
        public const string SelectQuery = "select * from {0}";
        public const string SelectById = "select * from {0} where id=@id";
        public const string SelectByCondition = "select * from {0} where {1}";

        public static string GenerateId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public enum Tables
        {
            users,
            cart,
            category,
            orders,
            products,
            cartitem
        }
    }
}
