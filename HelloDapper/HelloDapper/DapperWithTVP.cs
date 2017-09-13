using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDapper
{
    class DapperWithTVP
    {
        public void InsertMultipleRowsWODT()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                var existingtopsuppliers = conn
                    .Query<Supplier>("select top 2 CompanyName, City from Suppliers");
                DataTable dt = new DataTable();

                var parameters = existingtopsuppliers.AsTableValuedParameter("SupplierBasic", new string[] { "CompanyName", "City" });
                var spResult = conn.Execute("ImportBasicSuppliers", new { suppliers = parameters }, commandType: CommandType.StoredProcedure);
            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }
    }
}
