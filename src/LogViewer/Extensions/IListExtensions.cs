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
        /// <summary>
        /// The list must already be sorted by descending order.
        /// We will insert the new item and preserve the sortedness.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public static void InsertInDescendingOrder<T>(this IList<T> list, T item, Comparison<T> comparison)
        {
            Argument.IsNotNull(() => list);
            Argument.IsNotNull(() => item);
            Argument.IsNotNull(() => comparison);

            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }

            if(comparison(list.Last(), item) >= 0)
            {
                list.Add(item);
                return;
            }

            var index = 0;
            var lowerIndex = -1;

            while (index < list.Count && lowerIndex == -1)
            {
                var compareResult = comparison(list[index], item);

                if (compareResult < 0)
                {
                    lowerIndex = index;
                    break;
                }

                index++;
            }

            if (lowerIndex >= 0)
            {
                list.Insert(lowerIndex, item);
                return;
            }

            list.Add(item);
        }
    }
}