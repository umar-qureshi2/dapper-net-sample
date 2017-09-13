using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Collections;
using Dapper.Contrib.Extensions;
using Dapper.Contrib;

namespace HelloDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                var allSuppliers = conn.Query("ProductsWithSupplier",new {pid = 9 }, commandType: System.Data.CommandType.StoredProcedure);
                foreach (var supplier in allSuppliers)
                {
                    Console.WriteLine($"{supplier.SupplyInfo}");
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
    [Table("Suppliers")]
    public class Supplier : ISupplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        [Write(false)]
        public List<Product> Products { get; set; }
    }
}
