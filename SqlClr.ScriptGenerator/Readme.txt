Console application GenerateSqlScripts will generate sql script in order to create CLR SQL Server User-Defined Function

How to use it :
- it will need the path to a dll as a command line argument. DLL has to be compiled against .Net Framework 3.5 .

How it works : 
- it will scan the dll for types which have methods decorated with attribute : Microsoft.SqlServer.Server.SqlFunction 
and it will create sql script to create CLR SQL Server User-Defined Function

MSDN: http://msdn.microsoft.com/en-us/library/ms189876(v=sql.105).aspx 