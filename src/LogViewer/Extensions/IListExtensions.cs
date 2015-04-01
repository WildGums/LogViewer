// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.Collections.Generic;
    using Catel;

    public static class IListExtensions
    {
        public static void AddDescendingly<T>(this IList<T> list, T item, Comparison<T> compare)
        {            
            Argument.IsNotNull(() => list);
            Argument.IsNotNull(() => item);
            Argument.IsNotNull(() => compare);

            var count = list.Count;
            if (count == 0)
            {
                list.Add(item);
                return;
            }

            var index = 0;
            var lowerIndex = -1;

            while (index < count && lowerIndex == -1)
            {
                var compareResult = compare(list[index], item);
                if (compareResult < 0)
                {
                    lowerIndex = index;
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