using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Collections;

namespace HelloDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Execute(@"Update Suppliers SET ContactName = @sname WHERE SupplierID = @sid", new { sname = "Umar", sid = "2" });
            }

            //------------------Learn DB---------------------
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=learn;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();
                IEnumerable items = sqlConnection.Query("Select * from Item");
                foreach (var item in items)
                {
                    Console.WriteLine(); ObjectDumper.Write(item);
                }

                IEnumerable stores = sqlConnection.Query<Store>("Select * from Store where StoreId = @Id", new { Id = 1 });
                foreach (Store store in stores)
                {
                    Console.WriteLine(); ObjectDumper.Write(store);
                }
            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }
    }

    class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Id { get; set; }
        public int Store { get; set; }
    }

    class Store
    {
        public string StoreName { get; set; }
        public string StoreId { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        // reference 
        public Supplier Supplier { get; set; }
    }

    // Supplier
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public List<Product> Products { get; set; }
    }
}
