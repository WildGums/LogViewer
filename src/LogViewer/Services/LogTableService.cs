namespace LogViewer.Services
{
    using Models;

    public class LogTableService : ILogTableService
    {
        #region Constructors
        public LogTableService()
        {
            LogTable = new LogTable();
        }
        #endregion

        #region Properties
        public LogTable LogTable { get; private set; }
        #endregion
    }
}