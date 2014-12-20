namespace LogViewer.Models
{
    using Catel.Data;

    public abstract class SimplyClearableModel : ModelBase
    {
        public void MarkClean()
        {
            var oldValue = HandlePropertyAndCollectionChanges;
            HandlePropertyAndCollectionChanges = false;
            IsDirty = false;
            HandlePropertyAndCollectionChanges = oldValue;
        }
    }
}