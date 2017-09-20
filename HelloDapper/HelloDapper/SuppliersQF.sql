/* .sql query managed by QueryFirst add-in */


-- designTime - put parameter declarations and design time initialization here
DECLARE @name varchar(50) = 'a'

-- endDesignTime
SELECT * FROM Suppliers WHERE CompanyName LIKE '%' + @name --+ '%'
