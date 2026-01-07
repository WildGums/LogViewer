namespace LogViewer.Services
{
    public interface ILogTableConfigurationService
    {
        #region Methods
        bool GetIsTimestampVisible();
        void SetIsTimestampVisible(bool value);
        #endregion
    }
}