// NOTE: In case you were wondering, this code is from my own project here: https://github.com/gordon-matt/Extenso/
//  I have been a developer for over 10 years.. just not with gaming until now.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Assets.Scripts
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns a string containing the elements of source in CSV format.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a CSV formatted string from.</param>
        /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
        /// <returns>A string containing the elements of source in CSV format.</returns>
        public static string ToCsv<T>(this IEnumerable<T> source, bool outputColumnNames = true)
        {
            return ToDelimited(source, ",", outputColumnNames);
        }

        /// <summary>
        /// Returns a delimited string containing the elements of source.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a delimited string from.</param>
        /// <param name="delimiter">The character(s) used to separate the property values of each element in source.</param>
        /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
        /// <returns>A delimited string containing the elements of source.</returns>
        public static string ToDelimited<T>(this IEnumerable<T> source, string delimiter = ",", bool outputColumnNames = true)
        {
            var sb = new StringBuilder(2000);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (var propertyInfo in properties)
                {
                    sb.Append(propertyInfo.Name);
                    sb.Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Column Names

            #region Rows (Data)

            foreach (var element in source)
            {
                foreach (var propertyInfo in properties)
                {
                    string value = propertyInfo.GetValue(element).ToString().Replace("\"", "\"\"");
                    sb.Append(value.EnquoteDouble());
                    sb.Append(delimiter);
                }

                // Remove trailing comma
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Rows (Data)

            return sb.ToString();
        }
    }
}