// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Extensions
{
    using System;

    public static class StringExtensions
    {
        #region Methods
        public static string WrapToFindRegex(this string str)
        {
            return String.Format("(?={0})|(?<={0})", str);
        }

        public static string WrapToIgnoreCaseRegex(this string str)
        {
            return String.Format("(?i){0}(?-i)", str);
        }

        public static string WrapToMatchWholeWordRegex(this string str)
        {
            return string.Format(@"\b({0})\b", str);
        }
        #endregion
    }
}