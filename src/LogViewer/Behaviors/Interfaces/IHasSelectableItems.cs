namespace LogViewer.Behaviors
{
    using Models.Base;

    public interface IHasSelectableItems
    {
        NavigationNode SelectedItem { get; set; }
    }
}