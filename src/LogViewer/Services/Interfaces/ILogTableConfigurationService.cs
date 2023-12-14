namespace LogViewer.Services
{
    public interface ILogTableConfigurationService
    {
        #region Methods
        bool GetIsTimestampVisibile();
        void SetIsTimestampVisibile(bool value);
        #endregion
    }
}