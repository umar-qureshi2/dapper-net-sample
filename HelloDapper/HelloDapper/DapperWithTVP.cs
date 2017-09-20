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
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
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

        public void InsertWithDataTable()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                conn.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("CompanyName");
                dt.Columns.Add("City");
                dt.Rows.Add("New Washing Company", "Albuquerque");
                dt.Rows.Add("Walter White Inc.", "Albuquerque");
                conn.Execute("ImportBasicSuppliers", new { suppliers = dt.AsTableValuedParameter("SupplierBasic") }, commandType: CommandType.StoredProcedure);
            }
        }

        public void TVPWithMerge()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                conn.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("CompanyName");
                dt.Columns.Add("City");
                dt.Rows.Add("New Washing Company", "Marina City");
                dt.Rows.Add("Walter White & Jessie Inc.", "Albuquerque");
                conn.Execute("UpdateMergeSuppliers", new { suppliers = dt.AsTableValuedParameter("SupplierBasic") }, commandType: CommandType.StoredProcedure);

            }
        }
    }
}
