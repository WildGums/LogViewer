namespace LogViewer.Behaviors
{
    using Models.Base;

    public interface IHasSelectableItems
    {
        TreeNode SelectedItem { get; set; }
    }
}