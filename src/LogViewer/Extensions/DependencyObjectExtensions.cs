// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Extensions
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel.Windows;

    public static class DependencyObjectExtensions
    {
        public static IEnumerable<T> FindNestedVisualDescendantsByType<T>(this DependencyObject startElement) where T : DependencyObject
        {
            
            var stack = new Stack<DependencyObject>();
            stack.Push(startElement);
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                var children = current.GetVisualChildren();
                foreach (var child in children)
                {
                    var targetObject = child as T;
                    if (targetObject != null)
                    {
                        yield return targetObject;
                    }
                    
                    stack.Push(child);
                }
            }            
        }
    }
}