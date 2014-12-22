// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class ListExtensions
    {
        public static void RemoveByPredicate<T>(this IList<T> list, Predicate<T> predicate)
        {
            Argument.IsNotNull(() => predicate);

            var itemsToRemove = list.Where(x => predicate(x)).ToList();

            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
        }
    }
}