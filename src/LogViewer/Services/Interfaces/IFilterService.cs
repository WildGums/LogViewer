namespace LogViewer.Services
{
    using Models;

    public interface IFilterService
    {
        #region Properties
        Filter Filter { get; set; }
        #endregion

        #region Methods
        void ApplyFilesFilter();
        void ApplyLogRecordsFilter(FileNode fileNode = null);
        #endregion
    }
}