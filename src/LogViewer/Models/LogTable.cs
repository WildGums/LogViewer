namespace LogViewer.Models
{
    using Catel.Collections;
    using Catel.Data;

    public class LogTable : ModelBase
    {
        #region Constructors
        public LogTable()
        {
            Records = new FastObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public FastObservableCollection<LogRecord> Records { get; private set; }
        public bool IsTimestampVisible { get; set; }
        #endregion
    }
}