// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;

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
        #endregion
    }
}