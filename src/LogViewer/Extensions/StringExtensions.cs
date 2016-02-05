// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using Catel;

    public static class StringExtensions
    {
        #region Methods
        public static string WrapToFindRegex(this string str)
        {
            return string.Format("(?={0})|(?<={0})", str);
        }

        public static string WrapToIgnoreCaseRegex(this string str)
        {
            return string.Format("(?i){0}(?-i)", str);
        }

        public static string WrapToMatchWholeWordRegex(this string str)
        {
            return string.Format(@"\b({0})\b", str);
        }

        public static string ConvertWildcardToRegex(this string pattern)
        {
            Argument.IsNotNullOrEmpty(() => pattern);

            return "^" + Regex.Escape(pattern).
                Replace("\\*", ".*").
                Replace("\\?", ".") + "$";
        }

        public static bool IsSupportedFile(this string fullName, string regexFilter)
        {
            Argument.IsNotNullOrEmpty(() => fullName);
            Argument.IsNotNullOrEmpty(() => regexFilter);

            var fileName = Path.GetFileName(fullName);

            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return Regex.IsMatch(fileName, regexFilter);
        }

        public static bool IsFile(this string fullName)
        {
            return File.Exists(fullName);
        }

        public static bool IsDirectory(this string fullName)
        {
            return Directory.Exists(fullName);
        }
        #endregion
    }
}