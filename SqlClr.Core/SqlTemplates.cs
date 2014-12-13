using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlClr.Core
{
    public static class SqlTemplates
    {
        public const string SetUp = @"
        sp_configure 'show advanced options', 1
        RECONFIGURE
        GO
        sp_configure 'clr enabled' , 1
        RECONFIGURE
        GO

        -- Delete all functions from assembly '{0}'
        DECLARE @sql NVARCHAR(MAX)
        SET @sql = 'DROP FUNCTION ' + STUFF(
            (
                SELECT
                    ', ' + assembly_method 
                FROM
                    sys.assembly_modules
                WHERE
                    assembly_id IN (SELECT assembly_id FROM sys.assemblies WHERE name = '{0}')
                FOR XML PATH('')
            ), 1, 1, '')
        IF @sql IS NOT NULL EXEC sp_executesql @sql
        GO

         -- Delete existing assembly '{0}' if necessary
        IF EXISTS(SELECT 1 FROM sys.assemblies WHERE name = '{0}')
            DROP ASSEMBLY {0};
        GO

        {1}
        GO

        -- Create all functions from assembly '{0}'
        ";

        public const string CreateSqlAssemblyFromClrDll = @"
            -- Create assembly '{0}' from dll
            CREATE ASSEMBLY [{0}] 
                AUTHORIZATION [dbo]
                FROM 0x{1}
            ";

        public const string CreateSqlFunction = @"CREATE FUNCTION {0}({1}) RETURNS {2} A SEXTERNAL NAME {3};";

        public const string SqlFunctionExternalNameFormat = @"{0}.[{1}].{2}";
    }
}
