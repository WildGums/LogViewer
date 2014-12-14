namespace TreeViewMVVMInPlaceEditingDemo.Behaviors
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using Extensions;

  public class TreeViewInPlaceEditBehavior
  {
    #region Attached Properties
    #region IsEditable
    public static readonly DependencyProperty IsEditableProperty = DependencyProperty.RegisterAttached(
      "IsEditable", typeof (bool), typeof (TreeViewInPlaceEditBehavior), new PropertyMetadata(OnIsEditableChanged));

    public static bool GetIsEditable(DependencyObject obj)
    {
      return (bool) obj.GetValue(IsEditableProperty);
    }

    public static void SetIsEditable(DependencyObject obj, bool value)
    {
      obj.SetValue(IsEditableProperty, value);
    }
    #endregion IsEditable

    #region IsEditing
    public static readonly DependencyProperty IsEditingProperty = DependencyProperty.RegisterAttached(
      "IsEditing", typeof(bool), typeof(TreeViewInPlaceEditBehavior));

    public static bool GetIsEditing(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEditingProperty);
    }

    public static void SetIsEditing(DependencyObject obj, bool value)
    {
      obj.SetValue(IsEditingProperty, value);
    }
    #endregion IsEditing

    #region IsEditConfirmed
    public static readonly DependencyProperty IsEditConfirmedProperty = DependencyProperty.RegisterAttached(
      "IsEditConfirmed", typeof(bool), typeof(TreeViewInPlaceEditBehavior));

    public static bool GetIsEditConfirmed(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEditConfirmedProperty);
    }

    public static void SetIsEditConfirmed(DependencyObject obj, bool value)
    {
      obj.SetValue(IsEditConfirmedProperty, value);
    }
    #endregion IsEditConfirmed

    #region IsEditCancelled
    public static readonly DependencyProperty IsEditCancelledProperty = DependencyProperty.RegisterAttached(
      "IsEditCancelled", typeof(bool), typeof(TreeViewInPlaceEditBehavior));

    public static bool GetIsEditCancelled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEditCancelledProperty);
    }

    public static void SetIsEditCancelled(DependencyObject obj, bool value)
    {
      obj.SetValue(IsEditCancelledProperty, value);
    }
    #endregion IsEditCancelled

    #region LastSelectedItem
    private static readonly DependencyProperty LastSelectedItemProperty = DependencyProperty.RegisterAttached(
      "LastSelectedItem", typeof (object), typeof (TreeViewInPlaceEditBehavior));

    private static object GetLastSelectedItem(DependencyObject obj)
    {
      return obj.GetValue(LastSelectedItemProperty);
    }

    private static void SetLastSelectedItem(DependencyObject obj, object value)
    {
      obj.SetValue(LastSelectedItemProperty, value);
    }
    #endregion LastSelectedItem

    #region LastSelectedTime
    private static readonly DependencyProperty LastSelectedTimeProperty = DependencyProperty.RegisterAttached(
      "LastSelectedTime", typeof(DateTime), typeof(TreeViewInPlaceEditBehavior));

    private static DateTime GetLastSelectedTime(DependencyObject obj)
    {
      return (DateTime)obj.GetValue(LastSelectedTimeProperty);
    }

    private static void SetLastSelectedTime(DependencyObject obj, DateTime value)
    {
      obj.SetValue(LastSelectedTimeProperty, value);
    }
    #endregion LastSelectedTime
    #endregion Attached Properties

    #region Event Handlers
    private static void OnIsEditableChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      var treeView = obj as TreeView;
      if (treeView == null)
      {
        throw new ArgumentException("obj is not a TreeView");
      }

      treeView.PreviewKeyDown -= TreeViewPreviewKeyDown;
      treeView.PreviewMouseLeftButtonUp -= TreeViewPreviewMouseLeftButtonUp;
      treeView.SelectedItemChanged -= TreeViewSelectedItemChanged;
      if ((bool)args.NewValue)
      {
        treeView.PreviewKeyDown += TreeViewPreviewKeyDown;
        treeView.PreviewMouseLeftButtonUp += TreeViewPreviewMouseLeftButtonUp;
        treeView.SelectedItemChanged += TreeViewSelectedItemChanged;
      }
    }

    private static void TreeViewPreviewKeyDown(object sender, KeyEventArgs e)
    {
      var treeView = (TreeView) sender;
      switch (e.Key)
      {
        case Key.F2:
          treeView.BeginEdit();
          break;
        case Key.Escape:
          if (treeView.IsEditing())
          {
            treeView.EndEdit();
          }
          break;
        case Key.Return:
          if (treeView.IsEditing())
          {
            treeView.EndEdit(false);
          }
          break;
      }
    }

    private static void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var treeView = (TreeView)sender;
      var lastSelectedItem = GetLastSelectedItem(treeView);
      if (lastSelectedItem != treeView.SelectedItem)
      {
        ////Selection changed, let's save the selected item and the selected time
        SetLastSelectedItem(treeView, treeView.SelectedItem);
        SetLastSelectedTime(treeView, DateTime.Now);
        treeView.EndEdit();
      }
    }

    private static void TreeViewPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var treeView = (TreeView)sender;
      var element = (UIElement) e.OriginalSource;

      var selectedItem = element.ParentOfType<TreeViewItem>();
      if (selectedItem == null)
      {
        ////We're clicking on nowhere, let's cancel the editing
        treeView.EndEdit();
        return;
      }

      var lastSelectedItem = GetLastSelectedItem(treeView);
      if (lastSelectedItem == null || lastSelectedItem != treeView.SelectedItem)
      {
        return;
      }

      var lastSelctedTime = GetLastSelectedTime(treeView);
      var interval = DateTime.Now.Subtract(lastSelctedTime).TotalMilliseconds;
      if (interval >= 400 && interval <= 1200)
      {
        ////It's long double click, consider it as a edit sign 
        treeView.BeginEdit();
      }
    }
    #endregion Event Handlers
  }
}