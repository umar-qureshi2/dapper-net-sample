using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDapper
{
    public class DapperSqlBuilder
    {
        public void SelectById()
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-6Q7L361S\\MSSQLDEV;Initial Catalog=Northwind;User ID=sa;Password=Sa@123456"))
            {
                conn.Open();
                SqlBuilder builder = new SqlBuilder();
                var template = builder.AddTemplate("SELECT /**select**/ from Suppliers /**where**/");
                builder.Select("*");
                builder.Where("SupplierID = @SupplierID", new { SupplierID = 2 });
                var sql = template.RawSql;
                var supplier = conn.Query<Supplier>(sql, template.Parameters);
                ObjectDumper.Write(supplier);
            }
            Console.WriteLine("Please enter any key to exit");
            Console.ReadKey();
        }
    }
}
