﻿using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDapper
{
    class IntroductionCode
    {
        static void _tMain(string[] args)
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                sqlConnection.Open();

                IEnumerable products =
        sqlConnection.Query<Product>("Select * from Products");

                foreach (Product product in products)
                {
                    ObjectDumper.Write(product);
                }
            }
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=learn;User ID=sa;Password=Sa@123456"))
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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

    class DynamicDTO
    {
        public void _tmain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
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
    }

    class InsertUsingQuery
    {
        public void _tMain()
        {
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                var supplier = new Supplier()
                {
                    Address = "JLT",
                    CompanyName = "Trasix DMCC"
                };
                supplier.Id = sqlConnection.Query<int>(@"
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
            using (var sqlConnection = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                var supplier = new Supplier()
                {
                    Address = "JLT",
                    CompanyName = "Trasix DMCC"
                };
                supplier.Id = sqlConnection.Execute(@"
insert Suppliers(CompanyName, Address)
values (@CompanyName, @Address)
", supplier);
            }
        }
    }
}