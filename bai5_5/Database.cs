using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class Database
    {
        // Thêm ;TrustServerCertificate=True vào cuối chuỗi kết nối
        public static string ConnectionString = @"Data Source=DESKTOP-4U4DDI4\SQLEXPRESS;Initial Catalog=InventoryDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}