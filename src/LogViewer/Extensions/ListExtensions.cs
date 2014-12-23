// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public static async Task ClearOneByOne<T>(this IList<T> list)
        {
            while (list.Any())
            {
                list.RemoveAt(0);
            }
        }
        #endregion
    }
}