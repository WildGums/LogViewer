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
            ArgumentNullException.ThrowIfNull(predicate);

            var itemsToRemove = list.Where(x => predicate(x)).ToList();

            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
        }

        public static void ClearOneByOne<T>(this IList<T> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            while (list.Any())
            {
                list.RemoveAt(0);
            }
        }
        #endregion
    }
}