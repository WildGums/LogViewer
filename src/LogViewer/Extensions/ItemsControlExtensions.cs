namespace LogViewer
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Catel;

    public static class ItemsControlExtensions
    {
        public static IEnumerable<T> EnumerateNested<T>(this ItemsControl rootControl) 
            where T : ItemsControl
        {
            ArgumentNullException.ThrowIfNull(rootControl);

            var stack = new Queue<ItemsControl>();
            stack.Enqueue(rootControl);

            while (stack.Count != 0)
            {
                var item = stack.Dequeue();

                for (var i = 0; i < item.Items.Count; i++)
                {
                    var subItem = item.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                    if (subItem is not null)
                    {
                        stack.Enqueue(subItem);
                    }

                    var targetTyped = subItem as T;
                    if (targetTyped is not null)
                    {
                        yield return targetTyped;
                    }
                }
            }
        }
    }
}
