using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDapper
{
    class HelloDapper
    {
        static void _tMain(string[] args)
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();

                IEnumerable products =
        sqlConnection.Query<Product>("Select * from Products");

                foreach (Product product in products)
                {
                    ObjectDumper.Write(product);
                }
            }
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=learn;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                IEnumerable items = sqlConnection.Query("Select * from Item");
                foreach (var item in items)
                {
                    ObjectDumper.Write(item);
                }



                IEnumerable stores = sqlConnection.Query<Store>("Select * from Store");
                foreach (Store store in stores)
                {
                    ObjectDumper.Write(store);
                }
            }
            Console.ReadKey();
        }
    }

    class SelectiveQuery
    {
        static void _tMain(string[] args)
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();

                IEnumerable products =
        sqlConnection.Query<Product>(@"Select top 2 * from Products 
");
                //where ProductId = @ProductId", new { ProductId = 2 });

                foreach (Product product in products)
                {
                    Console.WriteLine(); ObjectDumper.Write(product);
                }
            }

            //------------------Learn DB---------------------
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=learn;User ID=sa;Password=sa123"))
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

    class DynamicDTO
    {
        public void _tmain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                Console.WriteLine();
                IEnumerable dynamicProducts = sqlConnection.Query("Select * from Products where ProductId = @Id", new { Id = 2 });
                foreach (dynamic product in dynamicProducts)
                {
                    Console.WriteLine($"{product.ProductID} - {product.ProductName}");
                }
            }
        }
    }

    class MultipleQuery
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                var multipleQueryResult = sqlConnection
                    .QueryMultiple("Select top 2 * from Products select top 2 * from Suppliers");
                using (multipleQueryResult)
                {
                    var productSet = multipleQueryResult.Read<Product>();
                    var supplierSet = multipleQueryResult.Read<Supplier>();
                    ObjectDumper.Write(productSet);
                    ObjectDumper.Write(supplierSet);
                    foreach (var supplier in supplierSet)
                    {
                        Console.WriteLine($"{supplier.Products?.Count()}");
                    }
                    if (!multipleQueryResult.IsConsumed)
                    {
                        var nullresult = multipleQueryResult.Read();
                    }
                }
            }
        }
    }

    class RetrieveReferencedObject
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                var productWithSupplier = sqlConnection.Query<Product, Supplier, Product>(
                    @"select Products.*, Suppliers.* from Products join Suppliers on Products.SupplierId = Suppliers.SupplierID and Suppliers.SupplierID = 2 and Products.ProductName = 'Louisiana Fiery Hot Pepper Sauce'"
, (a, s) =>
{
    a.Supplier = s;
    return a;
}
 , splitOn: "SupplierID");
                foreach (var item in productWithSupplier)
                {
                    Console.WriteLine();
                    ObjectDumper.Write(item);
                }
            }
        }

        public void _uMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                var supplierWithProducts = sqlConnection.Query<Supplier, Product, Supplier>(
                    @"select Suppliers.*,Products.* from Suppliers join Products on Products.SupplierId = Suppliers.SupplierID and Suppliers.SupplierID = 2"// and Products.ProductName = 'Louisiana Fiery Hot Pepper Sauce'"
, (supplier, product) =>
{
    supplier.Products = supplier.Products ?? new List<Product>();
    supplier.Products.Add(product);
    return supplier;
}
 , splitOn: "SupplierID");
                foreach (var item in supplierWithProducts)
                {
                    Console.WriteLine(item.Products?.Count);
                    ObjectDumper.Write(item);
                }
            }
        }

        public void ObjectsUsingMultiQuery()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Open();
                var supplierWithProducts = sqlConnection.QueryMultiple(
                    @"select * from Products where SupplierID = @sup; select * from Suppliers where SupplierID = @sup"
, new { sup = 2 });
                var allProducts = supplierWithProducts.Read<Product>();
                var supplier = supplierWithProducts.Read<Supplier>().Single();
                supplier.Products = new List<Product>(allProducts);

                Console.WriteLine(supplier.Products?.Count);
                ObjectDumper.Write(supplier);

            }
        }
        //complex and powerful
        //multiple store products retrieval
        public void DictionaryLookup()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                conn.Open();
                var suppliersLookup = new Dictionary<int, Supplier>();
                conn.Query<Supplier, Product, Supplier>(@"select Suppliers.*, Products.* from Products join Suppliers on Products.SupplierID = Suppliers.SupplierID and Suppliers.SupplierID IN @sid"
, (supplier, Product)
 =>
{
    if (!suppliersLookup.ContainsKey(supplier.SupplierID))
    {
        suppliersLookup.Add(supplier.SupplierID, supplier);
    }
    var tempSupplier = suppliersLookup[supplier.SupplierID];
    tempSupplier.Products = tempSupplier.Products ?? new List<Product>();
    tempSupplier.Products.Add(Product);
    return tempSupplier;
}, new { sid = new[] { 1, 2, 4 } }, splitOn: "SupplierID");
                foreach (var supplier in suppliersLookup.Values)
                {
                    Console.WriteLine(supplier.Products?.Count);
                    ObjectDumper.Write(supplier);
                }
            }
        }
    }

    class InsertUsingQuery
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                var supplier = new Supplier()
                {
                    Address = "JLT",
                    CompanyName = "Trasix DMCC"
                };
                supplier.SupplierID = sqlConnection.Query<int>(@"
insert Suppliers(CompanyName, Address)
values (@CompanyName, @Address)
select cast (scope_identity() as int)
", supplier).First();
            }
        }
    }

    class InsertUsingExecute
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                var supplier = new Supplier()
                {
                    Address = "JLT",
                    CompanyName = "Trasix DMCC"
                };
                supplier.SupplierID = sqlConnection.Execute(@"
insert Suppliers(CompanyName, Address)
values (@CompanyName, @Address)
", supplier);
            }
        }
    }

    class UpdateUsingExecute
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                sqlConnection.Execute(@"Update Suppliers SET ContactName = @sname WHERE SupplierID = @sid", new { sname = "Umar", sid = "2" });
            }
        }
    }

    class UsingDynamicParameters
    {
        public void _tMain()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@sid", 2, direction: System.Data.ParameterDirection.Input);
                parameters.Add("@newn", "Muhammad Umar Farooq", direction: System.Data.ParameterDirection.Input);
                string updateQuery = @"UPDATE SUPPLIERS set SUPPLIERS.ContactName = @newn where SUPPLIERS.SupplierID = @sid";
                var affected = conn.Execute(updateQuery, parameters);
                Console.Write($"affected rows = {affected}");
            }

        }
    }

    class MultiExecutionOfSignleStatement
    {
        public void _tMain()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=sa123"))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@sid", 2, direction: System.Data.ParameterDirection.Input);
                parameters.Add("@newn", "Muhammad Umar Farooq", direction: System.Data.ParameterDirection.Input);
                string updateQuery = @"UPDATE SUPPLIERS set SUPPLIERS.ContactName = @newn where SUPPLIERS.SupplierID = @sid";
                var affected = conn.Execute(updateQuery,
                    new[] {
                        new {sid=3,newn="name 3" },
                        new {sid=2,newn="name 2" },
                        new {sid=1, newn="muhammad umar farooq qureshi" }
                    });
                Console.Write($"affected rows = {affected}");
            }
        }
    }


}
