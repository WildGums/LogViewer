namespace TreeViewMVVMInPlaceEditingDemo.Extensions
{
  using System;
  using System.Windows;
  using System.Windows.Media;
  using Behaviors;

  /// <summary>
  /// Extension methods fo the DependencyObject objects
  /// </summary>
  public static class DependencyObjectExtension
  {
    /// <summary>
    /// Find a sequence of children of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <returns></returns>
    public static T ParentOfType<T>(this DependencyObject control) where T : DependencyObject
    {
      return ParentOfType<T>(control, null);
    }

    /// <summary>
    /// Find a sequence of children of type T and apply filter if applicable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="control"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static T ParentOfType<T>(this DependencyObject control, Predicate<T> filter) where T : DependencyObject
    {
      var parent = VisualTreeHelper.GetParent(control);

      var t = parent as T;
      return t != null && (filter == null || filter(t))
               ? t
               : (parent != null ? parent.ParentOfType(filter) : null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsEditing(this DependencyObject obj)
    {
      return TreeViewInPlaceEditBehavior.GetIsEditing(obj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static void BeginEdit(this DependencyObject obj)
    {
      TreeViewInPlaceEditBehavior.SetIsEditing(obj, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    public static void EndEdit(this DependencyObject obj, bool cancel = true)
    {
      TreeViewInPlaceEditBehavior.SetIsEditCancelled(obj, cancel);
      TreeViewInPlaceEditBehavior.SetIsEditConfirmed(obj, !cancel);
      TreeViewInPlaceEditBehavior.SetIsEditing(obj, false);
    }
  }
}