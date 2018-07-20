// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsControlExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Catel;

    public static class ItemsControlExtensions
    {
        #region Methods
        public static IEnumerable<T> EnumerateNested<T>(this ItemsControl rootControl) 
            where T : ItemsControl
        {
            Argument.IsNotNull(() => rootControl);

            var stack = new Queue<ItemsControl>();
            stack.Enqueue(rootControl);

            while (stack.Count != 0)
            {
                var item = stack.Dequeue();

                for (var i = 0; i < item.Items.Count; i++)
                {
                    var subItem = item.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                    if (subItem != null)
                    {
                        stack.Enqueue(subItem);
                    }

                    var targetTyped = subItem as T;
                    if (targetTyped != null)
                    {
                        yield return targetTyped;
                    }
                }
            }
        }
        #endregion
    }
}