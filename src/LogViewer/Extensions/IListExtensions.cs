namespace LogViewer
{
    using System;
    using System.Collections.Generic;

    public static class IListExtensions
    {
        #region Methods
        /// <summary>
        /// The list must already be sorted by descending order.
        ///  We will insert the new item and preserve the sortedness.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public static void InsertInAscendingOrder<T>(this IList<T> list, T item, Comparison<T> comparison) where T : class 
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(item);
            ArgumentNullException.ThrowIfNull(comparison);

            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }

            var index = 0;
            var greaterIndex = -1;

            while (index < list.Count && greaterIndex == -1)
            {
                var compareResult = comparison(list[index], item);

                if (compareResult > 0)
                {
                    greaterIndex = index;
                    break;
                }

                index++;
            }

            if (greaterIndex >= 0)
            {
                list.Insert(greaterIndex, item);
            }
            else
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// The list must already be sorted by descending order.
        /// We will insert the new item and preserve the sortedness.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparison"></param>
        public static void InsertInDescendingOrder<T>(this IList<T> list, T item, Comparison<T> comparison) where T : class 
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(item);
            ArgumentNullException.ThrowIfNull(comparison);

            if (list.Count == 0)
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
            }
            else
            {
                list.Insert(0, item);
            }
        }
        #endregion
    }
}