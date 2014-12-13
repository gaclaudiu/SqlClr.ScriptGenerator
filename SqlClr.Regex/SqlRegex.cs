using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SqlClr.Regex
{
    public class SqlRegex
    {
        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlString ReplaceByRegexPattern(SqlString input, SqlString pattern, SqlString replaceWith)
        {
            string data = input.IsNull ? null : input.Value;

            try
            {
                if (!input.IsNull && !pattern.IsNull && !string.IsNullOrEmpty(pattern.Value) && !replaceWith.IsNull)
                    data = System.Text.RegularExpressions.Regex.Replace(input.Value, pattern.Value, replaceWith.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error replacing with regex pattern " + ex.Message);
            }

            return new SqlString(data);
        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlBoolean SearchByRegexPattern(SqlString input, SqlString pattern)
        {
            try
            {
                if (!input.IsNull && !pattern.IsNull)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(input.Value, pattern.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled))
                        return SqlBoolean.True;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error searching by regex pattern " + ex.Message);
            }

            return SqlBoolean.False;
        }
    }
}
