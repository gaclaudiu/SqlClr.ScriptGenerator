using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqlClr.Core
{
    public class SqlScriptGenerator
    {
        public static string GenerateFromAssembly(string assemblyFilePath)
        {
            Assembly clrAssembly = Assembly.LoadFrom(assemblyFilePath);

            return GenerateSqlScript(clrAssembly);
        }

        private static string GenerateSqlScript(Assembly clrAssembly)
        {
            string assemblyName = clrAssembly.GetName().Name;

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(SqlTemplates.SetUp, assemblyName, GenerateAssemblyCreateSql(clrAssembly));

            foreach (Type classInfo in clrAssembly.GetTypes())
            {
                MethodInfo[] methods = classInfo.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);

                foreach (MethodInfo methodInfo in methods.Where( m => Attribute.IsDefined(m, typeof(SqlFunctionAttribute))))
                {
                    StringBuilder methodParameters = new StringBuilder();
                    bool firstParameter = true;
                    foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
                    {
                        if (firstParameter)
                            firstParameter = false;
                        else
                            methodParameters.Append(", ");

                        methodParameters.AppendFormat(@"@{0} {1}", paramInfo.Name, ConvertClrTypeToSql(paramInfo.ParameterType));
                    }

                    string returnType = ConvertClrTypeToSql(methodInfo.ReturnParameter.ParameterType);
                    string methodName = methodInfo.Name;
                    string className = (classInfo.Namespace == null ? "" : classInfo.Namespace + ".") + classInfo.Name;
                    string externalName = string.Format(SqlTemplates.SqlFunctionExternalNameFormat, assemblyName, className, methodName);

                    sql.AppendFormat(SqlTemplates.CreateSqlFunction, methodName, methodParameters, returnType, externalName)
                        .Append("\nGO\n");
                }
            }
            return sql.ToString();
        }

        private static string GenerateAssemblyCreateSql(Assembly clrAssembly)
        {
            StringBuilder bytes = new StringBuilder();
            using (FileStream dll = File.OpenRead(clrAssembly.Location))
            {
                int @byte;
                while ((@byte = dll.ReadByte()) >= 0)
                    bytes.AppendFormat("{0:X2}", @byte);
            }
            string sql = String.Format(SqlTemplates.CreateSqlAssemblyFromClrDll, clrAssembly.GetName().Name, bytes);

            return sql;
        }

        private static string ConvertClrTypeToSql(Type clrType)
        {
            switch (clrType.Name)
            {
                case "SqlString":
                    return "NVARCHAR(MAX)";
                case "SqlDateTime":
                    return "DATETIME";
                case "SqlInt16":
                    return "SMALLINT";
                case "SqlInt32":
                    return "INTEGER";
                case "SqlInt64":
                    return "BIGINT";
                case "SqlBoolean":
                    return "BIT";
                case "SqlMoney":
                    return "MONEY";
                case "SqlSingle":
                    return "REAL";
                case "SqlDouble":
                    return "DOUBLE";
                case "SqlDecimal":
                    return "DECIMAL(18,0)";
                case "SqlBinary":
                    return "VARBINARY(MAX)";
                default:
                    throw new ArgumentOutOfRangeException(clrType.Name + " is not a valid sql type.");
            }
        }
    }
}
