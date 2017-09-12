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
    public class DapperContrib
    {
        public void ReadByGet()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                var supplier = sqlConnection.Get<Supplier>(2);
            }
        }
        public void GetAll()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                var supplier = sqlConnection.GetAll<Supplier>();
            }
        }

        public void Update()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                var supplier = sqlConnection.Get<Supplier>(2);
                supplier.ContactName = "umar farooq";
                sqlConnection.Update(supplier);
            }
        }

        public void Insert()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                Supplier MegaSupplier = new Supplier()
                {
                    CompanyName = "Microsoft",
                    ContactName = "Satya Nadela"
                };
                var supplier = sqlConnection.Insert(MegaSupplier);
                Console.WriteLine(MegaSupplier.SupplierID);
            }
        }

        public void Delete()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                Supplier MegaSupplier = new Supplier()
                {
                    SupplierID = 36
                };
                var success = sqlConnection.Delete<Supplier>(MegaSupplier);
                Console.WriteLine(success);
            }
        }
    }

    // declare a interface for the object
    public interface ISupplier
    {

        int SupplierID { get; set; }

        string CompanyName { get; set; }
        string ContactName { get; set; }
        string ContactTitle { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string PostalCode { get; set; }
        string Country { get; set; }
        List<Product> Products { get; set; }
    }

}
