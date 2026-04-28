namespace LogViewer.Services
{
    using Models;

    public class LogTableService : ILogTableService
    {
        public LogTableService()
        {
            LogTable = new LogTable();
        }

        public LogTable LogTable { get; private set; }
    }
}
