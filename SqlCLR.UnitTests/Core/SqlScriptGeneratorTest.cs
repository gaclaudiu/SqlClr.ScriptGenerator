using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;
using SqlClr.Core;
using System.IO;

namespace SqlCLR.UnitTests.Core
{
    [TestClass]
    public class SqlScriptGeneratorTest
    {
        [TestMethod]
        public void GenerateFromAssembly_ScritpGenerated_Success()
        {
            string sqlScript = SqlScriptGenerator.GenerateFromAssembly("SqlClr.UnitTests.dll");

            Assert.IsTrue(!string.IsNullOrEmpty(sqlScript));
            Assert.IsTrue(sqlScript.Contains("'SqlCLR.UnitTests'"));
            Assert.IsTrue(sqlScript.Contains("SqlCLR.UnitTests.[SqlCLR.UnitTests.Core.TestSqlFunctionScriptGeneration].Test;"));
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GenerateFromAssembly_WrongDllPath_Error()
        {
            string sqlScript = SqlScriptGenerator.GenerateFromAssembly("NotFound.dll");
        }
    }

    public class TestSqlFunctionScriptGeneration
    {
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlString Test(SqlString inputOne, SqlString inputTwo) 
        {
            return new SqlString(string.Empty);
        }
    }
}
