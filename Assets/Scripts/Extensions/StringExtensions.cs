// NOTE: In case you were wondering, this code is from my own project here: https://github.com/gordon-matt/Extenso/
//  I have been a developer for over 10 years.. just not with gaming until now.

using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class StringExtensions
    {
        /// <summary>
        /// Encloses the given System.String in double quotes.
        /// </summary>
        /// <param name="source">The string to be enquoted.</param>
        /// <returns>A new System.String consisting of the original enquoted in double quotes.</returns>
        public static string EnquoteDouble(this string source)
        {
            return $"\"{source}\"";
        }

        /// <summary>
        /// Encloses the given System.String in single quotes.
        /// </summary>
        /// <param name="source">The string to be enquoted.</param>
        /// <returns>A new System.String consisting of the original enquoted in single quotes.</returns>
        public static string EnquoteSingle(this string source)
        {
            return $"'{source}'";
        }

        /// <summary>
        /// Splits the given string by newline characters and returns the result as a collection of strings.
        /// </summary>
        /// <param name="source">The string to split into lines.</param>
        /// <returns>A collection of strings.</returns>
        public static IEnumerable<string> ToLines(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return source.Split(new[] { "\r\n", Environment.NewLine, "\n" }, StringSplitOptions.None);
            }

            return new string[0];
        }
    }
}