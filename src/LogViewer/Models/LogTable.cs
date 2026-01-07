namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Collections;
    using Catel.Data;

    public class LogTable : ModelBase
    {
        public LogTable()
        {
            Records = new ObservableCollection<LogRecord>();
        }

        public ObservableCollection<LogRecord> Records { get; private set; }
        public bool IsTimestampVisible { get; set; }
    }
}
