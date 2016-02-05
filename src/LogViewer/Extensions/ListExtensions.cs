// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class ListExtensions
    {
        #region Methods
        public static void RemoveByPredicate<T>(this IList<T> list, Predicate<T> predicate)
        {
            Argument.IsNotNull(() => predicate);

            var itemsToRemove = list.Where(x => predicate(x)).ToList();

            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
        }

        public static void ClearOneByOne<T>(this IList<T> list)
        {
            Argument.IsNotNull(() => list);

            while (list.Any())
            {
                list.RemoveAt(0);
            }
        }
        #endregion
    }
}