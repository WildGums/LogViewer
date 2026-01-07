namespace LogViewer.Models
{
    using Catel.Data;

    public abstract class SimplyClearableModel : ModelBase
    {
        public void MarkClean()
        {
            IsDirty = false;
        }
    }
}
