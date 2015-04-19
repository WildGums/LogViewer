// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class IListExtensions
    {
        // See: http://stackoverflow.com/questions/1945461/how-do-i-sort-an-observable-collection

        public static void Sort<TSource, TKey>(this IList<TSource> source, Func<TSource, TKey> keySelector)
        {
            Argument.IsNotNull(() => source);
            Argument.IsNotNull(() => keySelector);

            var sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();

            foreach (var sortedItem in sortedList)
            {
                source.Add(sortedItem);
            }
        }

        public static void SortDescending<TSource, TKey>(this IList<TSource> source, Func<TSource, TKey> keySelector)
        {
            Argument.IsNotNull(() => source);
            Argument.IsNotNull(() => keySelector);

            var sortedList = source.OrderByDescending(keySelector).ToList();
            source.Clear();

            foreach (var sortedItem in sortedList)
            {
                source.Add(sortedItem);
            }
        }
    }
}