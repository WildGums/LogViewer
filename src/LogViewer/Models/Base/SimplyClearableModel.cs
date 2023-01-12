namespace LogViewer.Models
{
    using Catel.Data;

    public abstract class SimplyClearableModel : ModelBase
    {
        #region Methods
        public void MarkClean()
        {
            IsDirty = false;
        }
        #endregion
    }
}