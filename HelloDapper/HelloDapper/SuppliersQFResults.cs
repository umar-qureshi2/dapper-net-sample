using System;
using System.Data;
using System.Data.SqlClient;

namespace HelloDapper
{
    [Serializable]
    public partial class SuppliersQFResults
    {
        // Partial class extends the generated results class
        // Serializable by default, but you can change this here		
        // Put your methods here :-)
        internal void OnLoad()
        {
        }

        public static IDbConnection GetConnection()
        {
            return new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123");
        }
    }
}
