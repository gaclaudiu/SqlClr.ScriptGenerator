using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlClr.Regex;
using System.Data.SqlTypes;

namespace SqlCLR.UnitTests.Regex
{
    [TestClass]
    public class SqlRegexTest
    {
        [TestMethod]
        public void SearchByRegexPattern_MatchFound_Success()
        {
            SqlString data = new SqlString("This is a test. Test number 56. Is the 34 time running.");
            SqlString regularExpressionPattern = new SqlString("[0-9]");
            SqlBoolean result = SqlRegex.SearchByRegexPattern(data,regularExpressionPattern);

            Assert.IsFalse(result.IsNull);
            Assert.IsTrue(result.Value);
        }

        [TestMethod]
        public void SearchByRegexPattern_MatchNotFound_Success()
        {
            SqlString data = new SqlString("This is a test. Test number 56. Is the 34 time running.");
            SqlString regularExpressionPattern = new SqlString("2[0-4][0-9]");
            SqlBoolean result = SqlRegex.SearchByRegexPattern(data, regularExpressionPattern);

            Assert.IsFalse(result.IsNull);
            Assert.IsFalse(result.Value);
        }

        [TestMethod]
        public void SearchByRegexPattern_InputIsNull_Success()
        {
            SqlString regularExpressionPattern = new SqlString("2[0-4][0-9]");
            SqlBoolean result = SqlRegex.SearchByRegexPattern(null, regularExpressionPattern);

            Assert.IsFalse(result.IsNull);
            Assert.IsFalse(result.Value);
        }

        [TestMethod]
        public void SearchByRegexPattern_InputIsEmpty_Success()
        {
            SqlString data = new SqlString(string.Empty);
            SqlString regularExpressionPattern = new SqlString("2[0-4][0-9]");

            SqlBoolean result = SqlRegex.SearchByRegexPattern(data, regularExpressionPattern);

            Assert.IsFalse(result.IsNull);
            Assert.IsFalse(result.Value);
        }

        [TestMethod]
        public void SearchByRegexPattern_PatternIsEmpty_Success()
        {
            SqlString data = new SqlString("This is a test. Test number 56. Is the 34 time running.");
            SqlString regularExpressionPattern = new SqlString(string.Empty);

            SqlBoolean result = SqlRegex.SearchByRegexPattern(data, regularExpressionPattern);

            Assert.IsFalse(result.IsNull);
            Assert.IsTrue(result.Value);
        }

        [TestMethod]
        public void SearchByRegexPattern_PatternIsNull_Success()
        {
            SqlString data = new SqlString("This is a test. Test number 56. Is the 34 time running.");
            SqlString regularExpressionPattern = new SqlString(string.Empty);

            SqlBoolean result = SqlRegex.SearchByRegexPattern(data, null);

            Assert.IsFalse(result.IsNull);
            Assert.IsFalse(result.Value);
        }

        [TestMethod]
        public void ReplaceByRegexPattern_MatchFoundAndReplaced_Success()
        {
            SqlString data = new SqlString("This is a test. Test number 56. Is the 34 time running.");
            SqlString regularExpressionPattern = new SqlString("[0-9]");
            SqlString replacement = new SqlString("UT replacement");

            SqlString result = SqlRegex.ReplaceByRegexPattern(data, regularExpressionPattern, replacement);

            Assert.IsFalse(result.IsNull);
            Assert.IsTrue(result.Value.Contains(replacement.Value));
            Assert.IsFalse(result.Value.Contains("56"));
            Assert.IsFalse(result.Value.Contains("34"));
        }

        [TestMethod]
        public void ReplaceByRegexPattern_MatchNotFoundNoReplacement_Success()
        {
            SqlString data = new SqlString("This is a test.");
            SqlString regularExpressionPattern = new SqlString("[0-9]");
            SqlString replacement = new SqlString("UT replacement");

            SqlString result = SqlRegex.ReplaceByRegexPattern(data, regularExpressionPattern, replacement);

            Assert.IsFalse(result.IsNull);
            Assert.IsFalse(result.Value.Contains(replacement.Value));
            Assert.IsTrue(result.Value == data.Value);
        }

        [TestMethod]
        public void ReplaceByRegexPattern_InputIsNull_Success()
        {
            SqlString regularExpressionPattern = new SqlString("[0-9]");
            SqlString replacement = new SqlString("UT replacement");

            SqlString result = SqlRegex.ReplaceByRegexPattern(null, regularExpressionPattern, replacement);

            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void ReplaceByRegexPattern_PatternIsEmpty_Success()
        {
            SqlString data = new SqlString("This is a test.");
            SqlString regularExpressionPattern = new SqlString(string.Empty);
            SqlString replacement = new SqlString("UT replacement");

            SqlString result = SqlRegex.ReplaceByRegexPattern(data, regularExpressionPattern, replacement);

            Assert.IsFalse(result.IsNull);
            Assert.IsTrue(result.Value == data.Value);
        }

        [TestMethod]
        public void ReplaceByRegexPattern_PatternIsNull_Success()
        {
            SqlString data = new SqlString("This is a test.");
            SqlString regularExpressionPattern = new SqlString(string.Empty);
            SqlString replacement = new SqlString("UT replacement");

            SqlString result = SqlRegex.ReplaceByRegexPattern(data, null, replacement);

            Assert.IsFalse(result.IsNull);
            Assert.IsTrue(result.Value == data.Value);
        }


    }
}
