using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace HelloDapper
{
    public class DapperWithSP
    {
        public void GetAllSuppliers()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                var allSuppliers = conn.Query<Supplier>("AllSuppliers");
                foreach (var supplier in allSuppliers)
                {
                    Console.WriteLine($"SupplierID = {supplier.SupplierID}");
                }
            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }

        public void SpecificSupplier()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                var allSuppliers = conn.Query<Supplier>("SupplierByID", new { SupplierID = 2 }, commandType: System.Data.CommandType.StoredProcedure);
                foreach (var supplier in allSuppliers)
                {
                    Console.WriteLine($"SupplierID = {supplier.SupplierID}");
                }
            }
        }

        public void ProductWithSupplier()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                var allSuppliers = conn.Query("ProductsWithSupplier", new { pid = 9 }, commandType: System.Data.CommandType.StoredProcedure);
                foreach (var supplier in allSuppliers)
                {
                    Console.WriteLine($"{supplier.SupplyInfo}");
                }
            }
        }
    }
}
