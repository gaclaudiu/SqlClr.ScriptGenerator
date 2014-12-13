using Microsoft.SqlServer.Server;
using SqlClr.Core;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SqlClr.ScriptGenerator
{
    class Program
    {
        static string sqlFile = "Generated.sql";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("DLL path is missing.");
                Console.ReadLine();
                return;
            }

            string sql = SqlScriptGenerator.GenerateFromAssembly(args[0]);
            
            File.WriteAllText(sqlFile, sql);

            Console.WriteLine("Sql script generated to file Generated.sql .");
            Console.ReadLine();
        }
    }
}
